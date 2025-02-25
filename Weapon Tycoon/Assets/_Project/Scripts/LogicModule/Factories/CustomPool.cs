using System;
using System.Collections.Generic;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public abstract class CustomPool
    {
        [ShowInInspector] protected List<PooledView> _freeItems = new();

        public abstract PooledView Next();
    }
}