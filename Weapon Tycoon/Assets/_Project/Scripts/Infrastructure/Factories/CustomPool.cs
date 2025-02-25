using System;
using System.Collections.Generic;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;

namespace _Project.Scripts.Infrastructure.Factories
{
    [Serializable]
    public abstract class CustomPool
    {
        [ShowInInspector] protected List<PooledView> _freeItems = new();

        public abstract PooledView Next();
    }
}