using System;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Factories;

namespace _Project.Scripts
{
    public class BlasterFactoryResolver
    {
        private readonly Dictionary<Type, BlasterFactory> _factories;

        public BlasterFactoryResolver(Dictionary<Type, BlasterFactory> factories)
        {
            _factories = factories;
        }


    }
}