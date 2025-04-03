using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Storage;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace _Project.Scripts.Infrastructure.Pools
{
    public sealed class PoolAssetsLoader
    {
        private StorageService _storageService;
        
        public async UniTask<bool> LoadAssets(StorageService storageService,
            Dictionary<ObjectPoolService.SpawnerType, FastPool> spawnerPools,
            Dictionary<ObjectPoolService.EconomyType, FastPool> economyPools,
            Dictionary<ObjectPoolService.DefenceType, FastPool> defensePools)
        {
            _storageService = storageService;
            
            var task = LoadSpawnersAssets(spawnerPools);
            var task2 = LoadEconomyAssets(economyPools);
            var task3 = LoadDefenseAssets(defensePools);

            await UniTask.WhenAll(task, task2, task3);

            return true;
        }

        private async UniTask<IList<IResourceLocation>> LoadResourceLocationKeys(List<string> keys)
        {
            var locations
                = await _storageService.LoadResourceLocations<GameObject>(keys);
            
            return locations;
        }
        
        private async UniTask<bool> LoadSpawnersAssets(Dictionary<ObjectPoolService.SpawnerType, FastPool> spawnerPools)
        {
            var locations 
                = await LoadResourceLocationKeys(AssetReferenceKeys.AssetReferenceSpawnerKeys);

            if (locations == null)
                return false;

            foreach (IResourceLocation location in locations)
            {
                var handle = await _storageService.LoadFromAdressable<GameObject>(location);
                
                Enum.TryParse(location.PrimaryKey, out ObjectPoolService.SpawnerType spawnerType);
                spawnerPools[spawnerType].SetPrefab(handle.GetComponent<PooledView>());
            }

            return true;
        }

        private async UniTask<bool> LoadEconomyAssets(Dictionary<ObjectPoolService.EconomyType, FastPool> economyPools)
        {
            var locations 
                = await LoadResourceLocationKeys(AssetReferenceKeys.AssetReferenceEconomyKeys);

            if (locations == null)
                return false;

            foreach (IResourceLocation location in locations)
            {
                var handle = await Addressables.LoadAssetAsync<GameObject>(location).ToUniTask();

                Enum.TryParse(location.PrimaryKey, out ObjectPoolService.EconomyType economyType);
                economyPools[economyType].SetPrefab(handle.GetComponent<PooledView>());
            }
            
            return true;
        }
        
        private async UniTask<bool> LoadDefenseAssets(Dictionary<ObjectPoolService.DefenceType, FastPool> defensePools)
        {
            var locations 
                = await LoadResourceLocationKeys(AssetReferenceKeys.AssetReferenceDefenseKeys);

            if (locations == null)
                return false;

            foreach (IResourceLocation location in locations)
            {
                var handle = await Addressables.LoadAssetAsync<GameObject>(location).ToUniTask();

                Enum.TryParse(location.PrimaryKey, out ObjectPoolService.DefenceType defenceType);
                defensePools[defenceType].SetPrefab(handle.GetComponent<PooledView>());
            }
            
            return true;
        }
    }
}