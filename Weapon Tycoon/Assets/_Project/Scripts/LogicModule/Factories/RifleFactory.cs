using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class RifleFactory : BlasterFactory
    {
        public RifleFactory(StorageService prefab) : base(prefab)
        {
            _prefab = _storageService.GetWeaponView(WeaponStorage.WeaponType.Rifle1);
        }
    }
}