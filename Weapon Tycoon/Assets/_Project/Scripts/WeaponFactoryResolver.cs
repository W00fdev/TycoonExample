using System;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.LogicModule.Factories;

namespace _Project.Scripts
{
    public class WeaponFactoryResolver
    {
        private readonly Dictionary<Type, WeaponFactory> _factories;

        public WeaponFactoryResolver(Dictionary<Type, WeaponFactory> factories)
        {
            _factories = factories;
        }

        public void Resolve(WeaponSpawner spawner)
        {
            switch (spawner)
            {
                case GlockSpawner:
                    spawner.Resolve(_factories[typeof(GlockFactory)]);
                    break;
                case AkSpawner:
                    spawner.Resolve(_factories[typeof(AkFactory)]);
                    break;
            }
        }
    }
}