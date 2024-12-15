using System;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.Components
{
    public class PistolSpawner : BlasterSpawner
    {
        public override void Resolve(BlasterFactory blasterFactory)
        {
            if (blasterFactory is not PistolFactory)
                throw new InvalidCastException($"BlasterPistolSpawner got non BlasterPistolFactoryy {blasterFactory.GetType().Name}");
                    
            base.Resolve(blasterFactory);
        }
    }
}