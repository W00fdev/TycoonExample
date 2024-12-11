using System;
using _Project.Scripts.LogicModule.Factories;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class AkSpawner : WeaponSpawner
    {
        public override void Resolve(WeaponFactory weaponFactory)
        {
            if (weaponFactory is not AkFactory)
                throw new InvalidCastException($"AK spawner got non ak factory {weaponFactory.GetType().Name}");
                    
            base.Resolve(weaponFactory);
        }
    }
}