﻿using System;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class BoxFactory : CustomPool
    {
        private readonly PooledView _prefab;

        public Action<PooledView> BoxReturned;
        
        public BoxFactory(PooledView prefab) : base()
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
            
            var position = new Vector3(- 50 + UnityEngine.Random.value * 100, - 50 + UnityEngine.Random.value * 100,
                - 50 + UnityEngine.Random.value * 100);
            
            var weaponView = GameObject.Instantiate(_prefab, position, UnityEngine.Random.rotation);
            weaponView.ViewReturner += ReturnToItemsList;
         
            return weaponView;
        }

        private void ReturnToItemsList(PooledView pooled)
        {
            pooled.gameObject.SetActive(false);
            _freeItems.Add(pooled);
            BoxReturned?.Invoke(pooled);
        }
    }
}