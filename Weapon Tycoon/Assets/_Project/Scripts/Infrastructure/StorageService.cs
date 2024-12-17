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

        public static WeaponView GetWeaponView(WeaponStorage.WeaponType type)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.WaitForCompletion();
            return asyncOperationHandle.Result.GetComponent<WeaponView>();
        }

        public static PooledView GetBoxView(BoxStorage.BoxType type)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(type.ToString());
            asyncOperationHandle.WaitForCompletion();
            return asyncOperationHandle.Result.GetComponent<PooledView>();
        }
    }
}