using System;
using _Project.Scripts.LogicModule.Factories;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class ShotgunSpawner : BlasterSpawner
    {
        public override void Resolve(BlasterFactory blasterFactory)
        {
            if (blasterFactory is not ShotgunFactory)
                throw new InvalidCastException($"AK spawner got non shotgun factory {blasterFactory.GetType().Name}");
                    
            base.Resolve(blasterFactory);
        }
    }
}