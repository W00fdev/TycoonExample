using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class PistolFactory : BlasterFactory
    {
        protected PistolFactory(PooledView prefab) : base(prefab) { }
        
        public static async UniTask<BlasterFactory> CreateAsync(StorageService storageService) 
            => new PistolFactory(await storageService.GetWeaponViewAsync(BlasterType.Pistol1View));
    }
}