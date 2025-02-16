using System;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class RifleFactory : BlasterFactory
    {
        public RifleFactory() : base()
        {
            StorageService.GetWeaponView(BlasterType.Rifle1View, (x) => _prefab = x);
        }
    }
}