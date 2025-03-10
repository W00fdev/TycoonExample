﻿using System;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class BoxFactory : CustomPool
    {
        protected PooledView _prefab;

        protected BoxFactory(PooledView prefab)
            => _prefab = prefab;
        
        public static async UniTask<BoxFactory> CreateAsync(StorageService storageService) 
            => new BoxFactory(await storageService.GetBoxViewAsync(BoxType.Box));

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
            pooled.gameObject.SetActive(false);
            _freeItems.Add(pooled);
        }
    }
}