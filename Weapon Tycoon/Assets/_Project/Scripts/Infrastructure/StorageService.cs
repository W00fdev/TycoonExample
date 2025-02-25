using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Infrastructure
{
    /*public class AssetPath
    {
        public readonly string WeaponResourcesPath = "Blasters/";
        public readonly string BoxResourcesPath = "";
    }*/
    
    public class StorageService
    {
        public async UniTask<PooledView> GetWeaponViewAsync(BlasterType type)
        {
            var result = await Addressables.LoadAssetAsync<GameObject>(type.ToString())
                .Task.AsUniTask();
            return result.GetComponent<PooledView>();
        }
        
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void GetWeaponView(BlasterType type, Action<PooledView> onComplete)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            handle.Completed += (x) =>
            {
                onComplete?.Invoke(x.Result.GetComponent<PooledView>());
            };
        }
        
        public async UniTask<PooledView> GetBoxViewAsync(BoxType type)
        {
            var result = await Addressables.LoadAssetAsync<GameObject>(type.ToString())
                .Task.AsUniTask();
            return result.GetComponent<PooledView>();
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void GetBoxView(BoxType type, Action<PooledView> onComplete)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.Completed += (x) =>
            {
                onComplete?.Invoke(x.Result.GetComponent<PooledView>());
            };
        }
        
        public async UniTask<PooledView> GetMoneyTextPrefabAsync(string name)
        {
            var result = await Addressables.LoadAssetAsync<GameObject>(name)
                .Task.AsUniTask();
            return result.GetComponent<PooledView>();
        }
    }
}