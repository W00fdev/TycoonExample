using System;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.LogicModule.Factories;

namespace _Project.Scripts
{
    public class WeaponFactoryResolver
    {
        private readonly Dictionary<Type, BlasterFactory> _factories;

        public WeaponFactoryResolver(Dictionary<Type, BlasterFactory> factories)
        {
            _factories = factories;
        }

        public void Resolve(BlasterSpawner spawner)
        {
            switch (spawner)
            {
                case PistolSpawner:
                    spawner.Resolve(_factories[typeof(PistolFactory)]);
                    break;
                case ShotgunSpawner:
                    spawner.Resolve(_factories[typeof(ShotgunFactory)]);
                    break;
                case RifleSpawner:
                    spawner.Resolve(_factories[typeof(RifleFactory)]);
                    break;
            }
        }
    }
}