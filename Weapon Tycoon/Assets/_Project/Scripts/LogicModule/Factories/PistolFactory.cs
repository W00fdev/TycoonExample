using System;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class PistolFactory : BlasterFactory
    {
        public PistolFactory(WeaponView prefab) : base(prefab)
        {
        }
    }
}