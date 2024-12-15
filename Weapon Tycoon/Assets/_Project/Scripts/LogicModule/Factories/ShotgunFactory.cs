using System;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class ShotgunFactory : BlasterFactory
    {
        public ShotgunFactory(WeaponView prefab) : base(prefab)
        {
        }
    }
}