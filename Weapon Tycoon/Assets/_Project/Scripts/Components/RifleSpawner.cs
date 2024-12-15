using System;
using _Project.Scripts.LogicModule.Factories;

namespace _Project.Scripts.Components
{
    public class RifleSpawner : BlasterSpawner
    {
        public override void Resolve(BlasterFactory blasterFactory)
        {
            if (blasterFactory is not RifleFactory)
                throw new InvalidCastException($"RifleSpawner got non RifleFactory {blasterFactory.GetType().Name}");
                    
            base.Resolve(blasterFactory);
        }
    }
}