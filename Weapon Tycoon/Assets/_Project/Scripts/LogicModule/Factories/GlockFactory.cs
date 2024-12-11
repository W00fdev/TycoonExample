using System;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class GlockFactory : WeaponFactory
    {
        public GlockFactory(WeaponView prefab) : base(prefab)
        {
        }
    }
}