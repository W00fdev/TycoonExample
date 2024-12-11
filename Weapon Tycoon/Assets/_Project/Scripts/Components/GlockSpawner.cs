using System;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.Components
{
    public class GlockSpawner : WeaponSpawner
    {
        public override void Resolve(WeaponFactory weaponFactory)
        {
            if (weaponFactory is not GlockFactory)
                throw new InvalidCastException($"Glock spawner got non glock factory {weaponFactory.GetType().Name}");
                    
            base.Resolve(weaponFactory);
        }
    }
}