using System;
using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.BigBeautifulWall;
using _Project.Scripts.UI.Views.BigBeautifulWall;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

namespace _Project.Scripts.LogicModule.BigBeautifulWall
{
    public class Wall : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private WallInfoView _infoView;

        [SerializeField] protected ParticleSystem _upgradeImpact;
        [SerializeField] protected float _endScaleYSquash = 1.2f;
        [SerializeField] protected GameObject[] _upgradesVisual;

        [SerializeField] private string _wallNameKey;
        [SerializeField] private VolumePivot _volumePivot;

        private RestorableHealth _restorableHealth;
        private int _upgradeVisualLevel;
        private WallData _wallData;
        private Tween _deathTween;

        private Action _deactivateCallback;
        public bool IsInitialized { get; private set; }

        public RestorableHealth Health => _restorableHealth;
        
        public void Initialize(WallData wallData)
        {
            _wallData = wallData;
            
            _infoView.Initialize(_wallNameKey);
            _infoView.UpdateInfo(wallData.Regeneration.ToString(), wallData.Health.ToString());
            
            _upgradeVisualLevel = _wallData.Index;
            UpdateVisuals();
            
            _health.Initialize(wallData.Health);
            _restorableHealth = new RestorableHealth(this, _health, wallData.Health, wallData.Regeneration);
            
            _health.DamagedEvent += AnimateFeedback;
            _health.DiedEvent += Dead;

            _wallData.SpawnerDataChanged += UpgradeWall;
            
            IsInitialized = true;
        }

        public void LoadData(WallData wallData, PlayerData playerData)
        {
            int actualHealth = (playerData.WallActualHealth == -1) 
                ? wallData.Health 
                : playerData.WallActualHealth;
            
            _restorableHealth.UpgradeRegeneration(wallData.Regeneration);
            _restorableHealth.UpgradeMaxHealth(wallData.Health, notify: false);
            _restorableHealth.TakeDamage(Mathf.Abs(wallData.Health - actualHealth));
        }

        private void Dead()
        {
            if (_deathTween.isAlive)
                return;
            
            _infoView.HideInstant();
            _deathTween = Tween.ScaleY(transform, 0f, 0.5f, Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void Repair()
        {
            if (!_deathTween.isAlive)
            {
                gameObject.SetActive(true);
                Health.Repair();
                return;
            }
            
            _deathTween.Stop();
            Health.Repair();
            ShowAnimation();
        }

        private void OnEnable()
        {
            ShowAnimation();
        }

        private void ShowAnimation()
        {
            _infoView.ShowAsync().Forget();
            Tween.ScaleY(transform, 1f, 0.5f, Ease.OutBack);
        }

        private void OnDestroy() => _health.DamagedEvent -= AnimateFeedback;

        private void UpgradeWall()
        {
            float yPrevScale = transform.localScale.y;
            Sequence.Create(cycles: 1)
                .Chain(Tween.ScaleY(transform, _endScaleYSquash, 0.25f, Ease.InBack))
                .Chain(Tween.ScaleY(transform, yPrevScale, 0.25f, Ease.OutBack));
            
            _restorableHealth.UpgradeMaxHealth(_wallData.Health);
            _restorableHealth.UpgradeRegeneration(_wallData.Regeneration);
            _restorableHealth.Repair();
            
            _infoView.UpdateInfo(_wallData.Regeneration.ToString(), _wallData.Health.ToString());
            _upgradeVisualLevel = Mathf.Min(_upgradeVisualLevel + 1, _upgradesVisual.Length);
            _upgradeImpact.Play();

            _deactivateCallback = () => gameObject.SetActive(true);
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            for (int i = 0; i < Mathf.Min(_upgradeVisualLevel, _upgradesVisual.Length); i++)
                _upgradesVisual[i].SetActive(true);
        }
        
        private void AnimateFeedback() => Tween.PunchScale(transform, Vector3.up * 0.1f, 0.1f);
    }
}