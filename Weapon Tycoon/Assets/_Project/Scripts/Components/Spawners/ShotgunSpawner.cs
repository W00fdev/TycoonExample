using _Project.Scripts.Infrastructure.Data.Spawners;
using _Project.Scripts.Infrastructure.Pools;

namespace _Project.Scripts.Components.Spawners
{
    public class ShotgunSpawner : BlasterSpawner
    {
        private const int SpawnerIndex = 1;

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
            _blasterFactoryMethod = () => _poolService.Next(ObjectPoolService.SpawnerType.Shotgun1View);
            _boxFactoryMethod = () => _poolService.Next(ObjectPoolService.EconomyType.Box);

            base.Resolve();
        }
    }
}