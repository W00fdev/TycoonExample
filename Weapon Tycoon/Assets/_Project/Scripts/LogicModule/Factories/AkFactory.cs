using System;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class AkFactory : WeaponFactory
    {
        public AkFactory(WeaponView prefab) : base(prefab)
        {
        }
    }
}