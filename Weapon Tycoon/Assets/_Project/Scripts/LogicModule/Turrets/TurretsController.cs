using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.LogicModule.Spawners;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule.Turrets
{
    public class TurretsController : MonoBehaviour
    {
        [SerializeField] private TurretUpgrader[] _upgraders;
        private int _turretLevel;

        [Inject] private PersistentProgress _progress;
    
        public void Initialize()
        {
            var data = _progress.Data;
            if (data.Turrets < _upgraders.Length)
                _upgraders[data.Turrets].ShowBuyButton();
        
            for (int i = 0; i < _upgraders.Length; i++)
                _upgraders[i].Initialize(NextTurretButtonOpen, data.GetTurretUpgradeLevelOrDefault(i));
        }

        public void NextTurretButtonOpen()
        {
            if (_turretLevel >= _upgraders.Length)
                return;
        
            var upgrader = _upgraders[_turretLevel++];
            upgrader.ShowBuyButton();
        
            _progress.Data.Spawners = _turretLevel;
        }
    
        // Context invocation (BuySpawner Signal)
        public void BuyTurret(int turretIndex)
            => _upgraders[turretIndex].BuyTurret();
    
        // Context invocation (BuyUpgradeSpawner Signal)
        public void UpgradeTurret(int turretIndex) 
            => _upgraders[turretIndex].BuyUpgrade();
    }
}