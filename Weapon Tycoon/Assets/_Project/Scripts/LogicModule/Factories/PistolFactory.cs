using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class PistolFactory : BlasterFactory
    {
        public PistolFactory() : base()
        {
            StorageService.GetWeaponView(BlasterType.Pistol1View, 
                (x) =>
            {
                _prefab = x;
            });
            //StorageService.GetWeaponView(BlasterType.Pistol1View, (x) => _prefab = x);
        }
    }
}