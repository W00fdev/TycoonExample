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
            if (data.Spawners < _upgraders.Length)
                _upgraders[data.Spawners].ShowBuyButton();
            
            for (int i = 0; i < _upgraders.Length; i++)
                _upgraders[i].Initialize(NextSpawnerButtonOpen, data.GetSpawnerUpgradeLevelOrDefault(i));
        }

        public void NextSpawnerButtonOpen()
        {
            if (_spawnerLevel >= _upgraders.Length)
                return;
            
            var upgrader = _upgraders[_spawnerLevel++];
            upgrader.ShowBuyButton();
            
            _progress.Data.Spawners = _spawnerLevel;
        }
        
        // Context invocation (BuySpawner Signal)
        public void BuySpawner(int spawnerIndex)
            => _upgraders[spawnerIndex].BuySpawner();
        
        // Context invocation (BuyUpgradeSpawner Signal)
        public void UpgradeSpawner(int spawnerIndex) 
            => _upgraders[spawnerIndex].BuyUpgrade();
    }
}