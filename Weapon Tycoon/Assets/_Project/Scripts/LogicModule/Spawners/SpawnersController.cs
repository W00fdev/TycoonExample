using _Project.Scripts.Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule.Spawners
{
    public class SpawnersController : MonoBehaviour
    {
        [SerializeField] private SpawnerUpgrader[] _upgraders;
        private int _spawnerLevel;

        [Inject] private PersistentProgress _progress;
        
        public void Initialize()
        {
            var data = _progress.Data;
            int openSpawnersCount = data.SpawnerUpgrades.Count;
            if (openSpawnersCount < _upgraders.Length)
                _upgraders[openSpawnersCount].ShowBuyButton();
            
            for (int i = 0; i < _upgraders.Length; i++)
            {
                _upgraders[i].Initialize(NextSpawnerButtonOpen, data.GetSpawnerUpgradeLevelOrDefault(i));
                if (i < openSpawnersCount)
                    _upgraders[i].OpenOrLoad(data.GetTurretUpgradeLevelOrDefault(i));
            }
            
            _spawnerLevel = openSpawnersCount;
        }

        public void NextSpawnerButtonOpen()
        {
            if (_spawnerLevel + 1 >= _upgraders.Length)
                return;
            
            var upgrader = _upgraders[++_spawnerLevel];
            upgrader.ShowBuyButton();
        }
        
        // Context invocation (BuySpawner Signal)
        public void BuySpawner(int spawnerIndex)
            => _upgraders[spawnerIndex].BuySpawner();
        
        // Context invocation (BuyUpgradeSpawner Signal)
        public void UpgradeSpawner(int spawnerIndex) 
            => _upgraders[spawnerIndex].BuyUpgrade();
    }
}