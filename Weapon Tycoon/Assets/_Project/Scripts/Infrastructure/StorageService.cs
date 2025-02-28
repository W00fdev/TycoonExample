using System;
using _Project.Scripts.Components;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Infrastructure
{
    // ReSharper disable once ClassNeverInstantiated.Global
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
        
        public async UniTask<PooledView> GetProjectileViewAsync(ProjectileType type)
        {
            var result = await Addressables.LoadAssetAsync<GameObject>(type.ToString())
                .Task.AsUniTask();
            return result.GetComponent<PooledView>();
        }
        
        public async UniTask<PooledView> GetExplosionViewAsync(ExplosionType type)
        {
            var result = await Addressables.LoadAssetAsync<GameObject>(type.ToString())
                .Task.AsUniTask();
            return result.GetComponent<PooledView>();
        }
        
        public async UniTask<Enemy> GetEnemyPrefabAsync(EnemyType type)
        {
            var result = await Addressables.LoadAssetAsync<GameObject>(type.ToString())
                .Task.AsUniTask();
            return result.GetComponent<Enemy>();
        }
    }
}