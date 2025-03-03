using System;
using _Project.Scripts.Infrastructure.ScriptableEvents;
using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using _Project.Scripts.Utils;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Views.BigBeautifulWall
{
    public class WallHealthView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Transform _healthBar;
        [SerializeField] private Image _vignette;
        [SerializeField] private Image _redFill;
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private EventChannel _stopCameraMovement;
        [SerializeField] private EventChannel _resumeCameraMovement;

        private bool _isAllowedToDamageVisual;
        private const float DelayBeforeDamageVisual = 2f;

        private void Awake()
        {
            _isAllowedToDamageVisual = true;
        }

        public void UpdateHealthbar(int health, int maxHealth)
        {
            _healthText.text = "wall: " + health.ToHeaderMoneyFormat() + " / " + maxHealth.ToHeaderMoneyFormat();
            Tween.ScaleX(_healthBar, (float) health / maxHealth, 0.15f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B) && _isAllowedToDamageVisual)
            {
                ShowDamageAnimation().Forget();
            }
        }

        // FIX ME: maybe convert to coroutine?
        private async UniTaskVoid ShowDamageAnimation()
        {
            var vignetteTask = Tween.Alpha(_vignette, 0.65f, 0.1f, Ease.Linear).ToYieldInstruction().ToUniTask();
            var fillTask = Tween.Alpha(_redFill, 0.25f, 0.1f, Ease.Linear).ToYieldInstruction().ToUniTask();
            var shakeTask = Tween.ShakeLocalPosition(_mainCamera, Vector3.one * 0.35f, 0.1f).ToYieldInstruction().ToUniTask();

            _stopCameraMovement.Invoke(new Empty());

            _isAllowedToDamageVisual = false;
            
            await UniTask.WhenAll(vignetteTask, fillTask, shakeTask)
                .AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
            
            _resumeCameraMovement.Invoke(new Empty());
            
            Tween.Alpha(_vignette, 0f, 0.01f, Ease.Linear);
            Tween.Alpha(_redFill, 0f, 0.01f, Ease.Linear);

            await UniTask.Delay(TimeSpan.FromSeconds(DelayBeforeDamageVisual), 
                cancellationToken: this.GetCancellationTokenOnDestroy());

            _isAllowedToDamageVisual = true;
        }
    }
}