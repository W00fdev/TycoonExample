using System;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.UI.Models;

namespace _Project.Scripts.Components
{
    public class RifleSpawner : BlasterSpawner
    {
        private const int SpawnerIndex = 2;

        public override void Initialize(BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, SpawnerData spawnerData)
        {
            base.Initialize(boxFactory, moneyTextFactory, spawnerData);

            if (SpawnerIndex < PersistentProgress.Instance.SpawnerUpgrades.Count)
            {
                _upgradeVisualLevel = PersistentProgress.Instance.SpawnerUpgrades[SpawnerIndex];
                UpdateVisuals();
            }
        }

        public override void Resolve(BlasterFactory blasterFactory)
        {
            if (blasterFactory is not RifleFactory)
                throw new InvalidCastException($"RifleSpawner got non RifleFactory {blasterFactory.GetType().Name}");

            base.Resolve(blasterFactory);
        }
    }
}