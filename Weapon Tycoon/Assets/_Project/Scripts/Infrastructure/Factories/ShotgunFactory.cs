using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Factories
{
    [Serializable]
    public class ShotgunFactory : BlasterFactory
    {
        protected ShotgunFactory(PooledView prefab) : base(prefab) { }
        
        public static async UniTask<BlasterFactory> CreateAsync(StorageService storageService) 
            => new ShotgunFactory(await storageService.GetWeaponViewAsync(BlasterType.Shotgun1View));
    }
}