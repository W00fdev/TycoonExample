using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Factories
{
    [Serializable]
    public class RifleFactory : BlasterFactory
    {
        protected RifleFactory(PooledView prefab) : base(prefab) { }
        
        public static async UniTask<BlasterFactory> CreateAsync(StorageService storageService) 
            => new RifleFactory(await storageService.GetWeaponViewAsync(BlasterType.Rifle1View));
    }
}