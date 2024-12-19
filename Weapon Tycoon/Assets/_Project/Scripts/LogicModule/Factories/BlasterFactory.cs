﻿using System;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public abstract class BlasterFactory : CustomPool
    {
        protected BlasterView _prefab;
        
        public BlasterFactory(StorageService service) : base()
        {
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
            
            var weaponView = GameObject.Instantiate(_prefab);
            weaponView.ViewReturner += ReturnToItemsList;
         
            return weaponView;
        }

        private void ReturnToItemsList(PooledView pooled)
        {
            if (pooled is IEntity entity)
                EventBus.EntityConsumed?.Invoke(entity);
            
            pooled.gameObject.SetActive(false);
            _freeItems.Add(pooled);
        }
    }
}