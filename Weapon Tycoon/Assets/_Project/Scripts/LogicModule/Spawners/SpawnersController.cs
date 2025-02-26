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
            int spawnersCount = data.SpawnerUpgrades.Count;
            if (spawnersCount < _upgraders.Length)
                _upgraders[spawnersCount].ShowBuyButton();
            
            for (int i = 0; i < spawnersCount; i++)
                _upgraders[i].Initialize(NextSpawnerButtonOpen, data.GetSpawnerUpgradeLevelOrDefault(i));
        }

        public void NextSpawnerButtonOpen()
        {
            if (_spawnerLevel >= _upgraders.Length)
                return;
            
            var upgrader = _upgraders[_spawnerLevel++];
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