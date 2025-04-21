using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Views.BigBeautifulWall;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.LogicModule.BigBeautifulWall
{
    public class WallPresenter : MonoBehaviour
    {
        [SerializeField] private WallUpgrader _wallUpgrader;
        [SerializeField] private WallHealthView _wallHealthView;
        [SerializeField] private Wall _wall;

        private PersistentProgress _progress;
        
        public void Initialize(PersistentProgress progress)
        {
            _progress = progress;
            
            _wallHealthView.HideInstant();
            
            _wallUpgrader.WallOpened += ShowWallHealthbar;
            _wallUpgrader.Initialize(_progress);
            _wallUpgrader.LoadWall();
        }

        private void OnDestroy() => _wallUpgrader.Wall.Health.ChangedHealthEvent -= _wallHealthView.UpdateHealthbar;

        private void OnWallHealthChanged(int hp, int maxHp)
        {
            _wallHealthView.UpdateHealthbar(hp, maxHp);
            _progress.Data.WallActualHealth = hp;
            
            Debug.Log("Health saved: " + _progress.Data.WallActualHealth);
        }
        
        private void ShowWallHealthbar()
        {
            _wallUpgrader.WallOpened -= ShowWallHealthbar;
            _wallHealthView.ShowAsync().Forget();
            
            _wall.Health.ChangedHealthEvent += OnWallHealthChanged;
        }
    }
}