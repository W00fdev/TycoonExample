using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
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
        //private AssetPath _assetPath;
        
        // Addressables
        public static void GetWeaponView(BlasterType type, Action<PooledView> onComplete)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            handle.Completed += (x) =>
            {
                onComplete?.Invoke(x.Result.GetComponent<PooledView>());
            };
        }

        public static void GetBoxView(BoxType type, Action<PooledView> onComplete)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.Completed += (x) =>
            {
                onComplete?.Invoke(x.Result.GetComponent<PooledView>());
            };
        }
    }
}