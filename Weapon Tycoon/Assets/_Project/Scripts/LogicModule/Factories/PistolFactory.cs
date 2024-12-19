using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class PistolFactory : BlasterFactory
    {
        public PistolFactory(StorageService service) : base(service)
        {
            StorageService.GetWeaponView(BlasterType.Pistol1, (x) => _prefab = x);
        }
    }
}