using _Project.Scripts.Infrastructure.Data.Spawners;
using _Project.Scripts.Infrastructure.Pools;

namespace _Project.Scripts.Components.Spawners
{
    public class RifleSpawner : BlasterSpawner
    {
        private const int SpawnerIndex = 2;
        
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
            _blasterFactoryMethod = () => _poolService.Next(ObjectPoolService.SpawnerType.Rifle1View);
            _boxFactoryMethod = () => _poolService.Next(ObjectPoolService.EconomyType.LongBox);
            
            base.Resolve();
        }
    }
}