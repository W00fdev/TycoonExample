using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.LogicModule.BigBeautifulWall;
using _Project.Scripts.LogicModule.Turrets;
using _Project.Scripts.LogicModule.Waves;
using _Project.Scripts.UI.Presenters;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule
{
    public class DefenseShop : MonoBehaviour
    {
        [SerializeField] private CurrencyPipe _currencyPipe;
        
        [SerializeField] private TurretsController _turretsController;
        [SerializeField] private WavesSpawner _wavesSpawner;
        [SerializeField] private WallPresenter _wallPresenter;

        private PersistentProgress _progress;
        private EnemyFactory _enemyFactory;
        
        [Inject]
        public void Construct(PersistentProgress progress, EnemyFactory enemyFactory)
        {
            _progress = progress;
            _enemyFactory = enemyFactory;
        }
        
        public void Initialize()
        {
            _turretsController.Initialize(_progress);
            _wavesSpawner.Initialize(_enemyFactory);
            _wallPresenter.Initialize(_progress);
        }
    }
}