using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Views
{
    public class PooledView : MonoBehaviour, IPooled
    {
        public Action<PooledView> ViewReturner { get; set; }
        
        public virtual void ReturnToPool()
        {
            ViewReturner?.Invoke(this);
        }

        [Button("Consume")]
        public void ConsumeProduct()
        {
            ReturnToPool();
        }
    }
}