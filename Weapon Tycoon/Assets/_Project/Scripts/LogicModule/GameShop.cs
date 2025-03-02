using _Project.Scripts.LogicModule.BigBeautifulWall;
using _Project.Scripts.LogicModule.Spawners;
using _Project.Scripts.LogicModule.Turrets;
using _Project.Scripts.LogicModule.Waves;
using _Project.Scripts.UI.Presenters;
using UnityEngine;

namespace _Project.Scripts.LogicModule
{
    public class GameShop : MonoBehaviour
    {
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private SpawnersController _spawnersController;
        [SerializeField] private TurretsController _turretsController;
        [SerializeField] private WavesController _wavesController;
        [SerializeField] private WallUpgrader _wallUpgrader;
        
        public void Initialize()
        {
            _spawnersController.Initialize();
            _turretsController.Initialize();
            _wavesController.Initialize();
            
            _wallUpgrader.Initialize(null);
        }
    }
}