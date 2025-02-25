using System;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.Spawners;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.UI.Models;
using Zenject;

namespace _Project.Scripts.Components
{
    public class ShotgunSpawner : BlasterSpawner
    {
        private const int SpawnerIndex = 1;
        [Inject] private PersistentProgress _progress;

        public override void Initialize(BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, SpawnerData spawnerData)
        {
            base.Initialize(boxFactory, moneyTextFactory, spawnerData);

            if (SpawnerIndex < _progress.Data.SpawnerUpgrades.Count)
            {
                _upgradeVisualLevel = _progress.Data.SpawnerUpgrades[SpawnerIndex];
                UpdateVisuals();
            }
        }

        public override void Resolve(BlasterFactory blasterFactory)
        {
            if (blasterFactory is not ShotgunFactory)
                throw new InvalidCastException($"AK spawner got non shotgun factory {blasterFactory.GetType().Name}");

            base.Resolve(blasterFactory);
        }
    }
}