using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Infrastructure
{
    public class StorageService
    {
        public static void GetWeaponView(BlasterType type, Action<BlasterView> onComplete)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.Completed += (x) => onComplete?.Invoke(x.Result.GetComponent<BlasterView>()); 
        }

        public static void GetBoxView(BoxType type, Action<PooledView> onComplete)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.Completed += (x) => onComplete?.Invoke(x.Result.GetComponent<PooledView>()); 
        }
    }
}