using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Infrastructure
{
    public class StorageService
    {
        // private readonly WeaponStorage _weaponStorage;
        // private readonly BoxStorage _boxStorage;
        
        public StorageService()
        {
            // _weaponStorage = Resources.Load<WeaponStorage>(AssetPath.StoragesPath + "WeaponStorage");
            // _boxStorage = Resources.Load<BoxStorage>(AssetPath.StoragesPath + "BoxStorage");
        }

        public static void GetWeaponView(WeaponStorage.WeaponType type, Action<WeaponView> onComplete)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.Completed += (x) => onComplete?.Invoke(x.Result.GetComponent<WeaponView>()); 
        }

        public static void GetBoxView(BoxStorage.BoxType type, Action<PooledView> onComplete)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.Completed += (x) => onComplete?.Invoke(x.Result.GetComponent<PooledView>()); 
        }
    }
}