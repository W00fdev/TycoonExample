using System;
using System.Threading;
using _Project.Scripts.Infrastructure.ScriptableEvents;
using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using _Project.Scripts.UI.Dialogs;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Views.BigBeautifulWall
{
    public class WallHealthView : PopupDialog
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Transform _healthBar;
        [SerializeField] private Image _vignette;
        [SerializeField] private Image _redFill;
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private EventChannel _stopCameraMovement;
        [SerializeField] private EventChannel _resumeCameraMovement;

        private CancellationTokenSource _cts;
        private CancellationTokenSource _linkedCts;
        
        private int _previousHealth;
        private bool _isAllowedToDamageVisual;
        private const float DelayBeforeDamageVisual = 2f;
        private const float NonZeroOffset = 0.0001f;
        
        public bool IsPlaying => _isPlaying;

        private void Awake()
        {
            _isAllowedToDamageVisual = true;
        }

        public void UpdateHealthbar(int health, int maxHealth)
        {
            if (_isPlaying)
                InterruptAnimation();
            
            if (_previousHealth > 0 && health <= 0)
            {
                HideAsync().Forget();
            }
            else if (health < _previousHealth)
            {
                ShowDamageAnimation().Forget();
            }
            else if (_previousHealth <= 0 && health > 0)
            {
                ShowAsync().Forget();
            }
            
            _healthText.text = "wall: " + health + " / " + maxHealth;
            Tween.ScaleX(_healthBar, health / (maxHealth + NonZeroOffset), 0.15f);

            _previousHealth = health;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B) && _isAllowedToDamageVisual)
            {
                ShowDamageAnimation().Forget();
            }
        }
#endif

        // FIX ME: maybe convert to coroutine?
        public async UniTaskVoid ShowDamageAnimation()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;
            
            var vignetteTask = Tween.Alpha(_vignette, 0.65f, 0.1f, Ease.Linear).ToYieldInstruction().ToUniTask();
            var fillTask = Tween.Alpha(_redFill, 0.25f, 0.1f, Ease.Linear).ToYieldInstruction().ToUniTask();
            var shakeTask = Tween.ShakeLocalPosition(_mainCamera, Vector3.one * 0.15f, 0.1f).ToYieldInstruction()
                .ToUniTask();

            _stopCameraMovement.Invoke(new Empty());

            _isAllowedToDamageVisual = false;

            await UniTask.WhenAll(vignetteTask, fillTask, shakeTask)
                .AttachExternalCancellation(GetCancellationToken());

            _resumeCameraMovement.Invoke(new Empty());

            Tween.Alpha(_vignette, 0f, 0.01f, Ease.Linear);
            Tween.Alpha(_redFill, 0f, 0.01f, Ease.Linear);

            await UniTask.Delay(TimeSpan.FromSeconds(DelayBeforeDamageVisual),
                cancellationToken: GetCancellationToken());

            _isAllowedToDamageVisual = true;
            _isPlaying = false;
        }

        private void InterruptAnimation()
        {
            _linkedCts.Cancel();
            _isPlaying = false;
        }

        protected override CancellationToken GetCancellationToken()
        {
            _cts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, base.GetCancellationToken());
            return _linkedCts.Token;
        }
    }
}