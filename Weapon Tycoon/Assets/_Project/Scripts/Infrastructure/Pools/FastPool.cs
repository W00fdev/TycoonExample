using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.LogicModule.Views;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Infrastructure.Pools
{
    [Serializable]
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)] 
    public sealed class FastPool : IPool
    {
        [ShowInInspector] private Stack<PooledView> _freeItems;

        private readonly Transform _root;
        private PooledView _prefab;

        public FastPool(Transform root) => _root = root;

        public void SetPrefab(PooledView prefab)
            => _prefab = prefab;

        [Button]
        public PooledView Next()
        {
            if (_freeItems.Count > 0)
            {
                var item = _freeItems.Pop();
                item.gameObject.SetActive(true);
                
                return item;
            }
            
            var instance = Object.Instantiate(_prefab, _root);
            instance.ViewReturner += ReturnToItemsList;
         
            return instance;
        }

        public void Warm(int count)
        {
            _freeItems ??= new Stack<PooledView>(count);
            
            for (int i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(_prefab, _root);
                instance.gameObject.SetActive(false);
            
                _freeItems.Push(instance);                
            }
        }

        private void ReturnToItemsList(PooledView pooled)
        {
            pooled.gameObject.SetActive(false);
            _freeItems.Push(pooled);
        }
    }
}