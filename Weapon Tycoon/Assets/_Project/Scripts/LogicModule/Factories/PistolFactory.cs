using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class PistolFactory : BlasterFactory
    {
        public PistolFactory(StorageService prefab) : base(prefab)
        {
            _prefab = _storageService.GetWeaponView(WeaponStorage.WeaponType.Pistol1);
        }
    }
}