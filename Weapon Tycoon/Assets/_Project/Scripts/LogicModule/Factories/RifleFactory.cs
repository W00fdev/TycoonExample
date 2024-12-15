using System;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class RifleFactory : BlasterFactory
    {
        public RifleFactory(WeaponView prefab) : base(prefab)
        {
        }
    }
}