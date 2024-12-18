using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class RifleFactory : BlasterFactory
    {
        public RifleFactory(StorageService service) : base(service)
        {
            StorageService.GetWeaponView(WeaponStorage.WeaponType.Rifle1, (x) => _prefab = x);
        }
    }
}