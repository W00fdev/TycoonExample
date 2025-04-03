using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Infrastructure.Storage;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Infrastructure.Pools
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ObjectPoolService
    {
        private readonly Dictionary<SpawnerType, FastPool> _spawnerPools;
        private readonly Dictionary<EconomyType, FastPool> _economyPools;
        private readonly Dictionary<DefenceType, FastPool> _defensePools;
        private readonly PoolAssetsLoader _assetsLoader;
        private readonly StorageService _storageService;

        private const int WarmCount = 10;
        
        public enum SpawnerType : byte
        {
            Pistol1View,
            Shotgun1View,
            Rifle1View,
        }

        public enum EconomyType : byte
        {
            Box,
            LongBox,
            MoneyTextView,
        }

        public enum DefenceType : byte
        {
            Projectile,
            LaserYellow,
            LaserExplosionYellow,
            Enemy1,
        }

        public ObjectPoolService(StorageService storageService, Transform parent)
        {
            _storageService = storageService;
            _assetsLoader = new PoolAssetsLoader();

            Transform root = new GameObject("Pool").transform;
            root.SetParent(parent);

            // Can be changed to Reflection.
            _spawnerPools = new()
            {
                { SpawnerType.Pistol1View, new FastPool(root) },
                { SpawnerType.Shotgun1View, new FastPool(root) },
                { SpawnerType.Rifle1View, new FastPool(root) },
            };
            
            _economyPools = new()
            {
                { EconomyType.Box, new FastPool(root) },
                { EconomyType.LongBox, new FastPool(root) },
                { EconomyType.MoneyTextView, new FastPool(root) },
            };
            
            _defensePools = new()
            {
                { DefenceType.Projectile, new FastPool(root) },
                { DefenceType.LaserYellow, new FastPool(root) },
                { DefenceType.LaserExplosionYellow, new FastPool(root) },
                { DefenceType.Enemy1, new FastPool(root) },
            };
        }

        public UniTask<bool> LoadAssets()
            => _assetsLoader.LoadAssets(_storageService, _spawnerPools, _economyPools, _defensePools);

        public void Warm()
        {
            foreach(var spawner in _spawnerPools)
                spawner.Value.Warm(WarmCount);
            
            foreach(var spawner in _economyPools)
                spawner.Value.Warm(WarmCount);

            foreach(var spawner in _defensePools)
                spawner.Value.Warm(WarmCount);
        }
        
        public PooledView Next(SpawnerType type)
            => _spawnerPools[type].Next();
        
        public PooledView Next(EconomyType type)
            => _economyPools[type].Next();
        
        public PooledView Next(DefenceType type)
            => _defensePools[type].Next();
    }
}