using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Data.BigBeautifulWall;
using _Project.Scripts.UI.Views.BigBeautifulWall;
using PrimeTween;
using UnityEngine;

namespace _Project.Scripts.LogicModule.BigBeautifulWall
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private RestorableHealth _health;
        [SerializeField] private WallInfoView _infoView;

        [SerializeField] protected ParticleSystem _upgradeImpact;
        [SerializeField] protected float _endScaleYSquash = 1.2f;
        [SerializeField] protected GameObject[] _upgradesVisual;

        [SerializeField] private string _wallNameKey;
        
        protected int _upgradeVisualLevel;
        private WallData _wallData;
        
        public virtual void Initialize(WallData wallData)
        {
            _wallData = wallData;
            
            _infoView.Initialize(_wallNameKey);
            _infoView.UpdateInfo(wallData.Regeneration.ToString(), wallData.Health.ToString());
            
            _upgradeVisualLevel = _wallData.Index;
            UpdateVisuals();

            _wallData.SpawnerDataChanged += UpgradeWall;
        }
        
        private void UpgradeWall()
        {
            float yPrevScale = transform.localScale.y;
            Sequence.Create(cycles: 1)
                .Chain(Tween.ScaleY(transform, _endScaleYSquash, 0.25f, Ease.InBack))
                .Chain(Tween.ScaleY(transform, yPrevScale, 0.25f, Ease.OutBack));
            
            _infoView.UpdateInfo(_wallData.Regeneration.ToString(), _wallData.Health.ToString());
            _upgradeVisualLevel = Mathf.Min(_upgradeVisualLevel + 1, _upgradesVisual.Length);
            _upgradeImpact.Play();

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            for (int i = 0; i < Mathf.Min(_upgradeVisualLevel, _upgradesVisual.Length); i++)
                _upgradesVisual[i].SetActive(true);
        }
        
        private void TakeDamage() => Tween.PunchScale(transform, Vector3.up * 0.1f, 0.1f);
    }
}