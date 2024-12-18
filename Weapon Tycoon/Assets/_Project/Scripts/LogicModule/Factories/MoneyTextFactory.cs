using System;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class MoneyTextFactory : CustomPool
    {
        private readonly PooledView _prefab;

        public Action<PooledView> MoneyReturned;
        
        public MoneyTextFactory(PooledView prefab) : base()
        {
            _prefab = prefab;
        }

        [Button]
        public override PooledView Next()
        {
            if (_freeItems.Count > 0)
            {
                var item = _freeItems[0];
                item.gameObject.SetActive(true);
                _freeItems.RemoveAt(0);
                
                return item;
            }
            
            var pooledView = GameObject.Instantiate(_prefab);
            pooledView.ViewReturner += ReturnToItemsList;
         
            return pooledView;
        }

        private void ReturnToItemsList(PooledView pooled)
        {
            MoneyReturned?.Invoke(pooled);
            
            pooled.gameObject.SetActive(false);
            _freeItems.Add(pooled);
        }
    }
}