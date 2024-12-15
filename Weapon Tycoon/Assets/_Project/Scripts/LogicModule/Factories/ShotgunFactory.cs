using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class ShotgunFactory : BlasterFactory
    {
        public ShotgunFactory(StorageService prefab) : base(prefab)
        {
            _prefab = _storageService.GetWeaponView(WeaponStorage.WeaponType.Shotgun1);
        }
    }
}