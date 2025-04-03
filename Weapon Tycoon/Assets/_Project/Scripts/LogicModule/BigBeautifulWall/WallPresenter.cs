using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Views.BigBeautifulWall;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule.BigBeautifulWall
{
    public class WallPresenter : MonoBehaviour
    {
        [SerializeField] private WallUpgrader _wallUpgrader;
        [SerializeField] private WallHealthView _wallHealthView;
        [SerializeField] private Wall _wall;

        [Inject] private PersistentProgress _progress;
        
        public void Initialize()
        {
            _wallHealthView.HideInstant();

            _wall.Health.ChangedHealthEvent += OnWallHealthChanged;

            _wallUpgrader.WallOpened += ShowWallHealthbar;
            _wallUpgrader.Initialize();
        }

        private void OnDestroy() => _wallUpgrader.Wall.Health.ChangedHealthEvent -= _wallHealthView.UpdateHealthbar;

        private void OnWallHealthChanged(int hp, int maxHp)
        {
            _wallHealthView.UpdateHealthbar(hp, maxHp);
            _progress.Data.WallActualHealth = hp;
        }
        
        private void ShowWallHealthbar()
        {
            _wallUpgrader.WallOpened -= ShowWallHealthbar;
            _wallHealthView.ShowAsync().Forget();
        }
    }
}