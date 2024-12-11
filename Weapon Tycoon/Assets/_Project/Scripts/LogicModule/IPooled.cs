using System;
using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.LogicModule
{
    public interface IPooled
    {
        Action<PooledView> ViewReturner { get; set; }

        void ReturnToPool();
    }
    
    
}