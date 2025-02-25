using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Factories
{
    [Serializable]
    public class MoneyTextFactory : CustomPool
    {
        private readonly PooledView _prefab;

        protected MoneyTextFactory(PooledView prefab)
            => _prefab = prefab;
        
        public static async UniTask<MoneyTextFactory> CreateAsync(StorageService storageService) 
            => new MoneyTextFactory(await storageService.GetMoneyTextPrefabAsync("MoneyTextView"));

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
            pooled.gameObject.SetActive(false);
            _freeItems.Add(pooled);
        }
    }
}