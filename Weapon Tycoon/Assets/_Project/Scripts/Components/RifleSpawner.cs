using System;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.Spawners;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Factories.Accessors;
using _Project.Scripts.UI.Models;
using Zenject;

namespace _Project.Scripts.Components
{
    public class RifleSpawner : BlasterSpawner
    {
        private const int SpawnerIndex = 2;

        [Inject] private PersistentProgress _progress;
        
        [Inject] private RifleFactoryAccessor<RifleFactory> _rifleFactoryAccessor;
        [Inject] private BoxFactoryAccessor<LongBoxFactory> _longBoxFactoryAccessor;
        [Inject] private MoneyTextFactoryAccessor _moneyTextFactoryAccessor;
        
        public override void Initialize(SpawnerData spawnerData)
        {
            base.Initialize(spawnerData);

            if (SpawnerIndex < _progress.Data.SpawnerUpgrades.Count)
            {
                _upgradeVisualLevel = _progress.Data.SpawnerUpgrades[SpawnerIndex];
                UpdateVisuals();
            }
        }

        public override void Resolve()
        {
            _blasterFactory = _rifleFactoryAccessor.RifleFactory;
            _boxFactory = _longBoxFactoryAccessor.BoxFactory;
            _moneyTextFactory = _moneyTextFactoryAccessor.MoneyTextFactory;
            
            base.Resolve();
        }
    }
}