using System;
using _Project.Scripts.Components;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Factories
{
    [Serializable]
    public class EnemyFactory : CustomPool
    {
        protected Enemy _prefab;
        
        protected EnemyFactory(Enemy prefab)
            => _prefab = prefab;
        
        public static async UniTask<EnemyFactory> CreateAsync(StorageService storageService) 
            => new EnemyFactory(await storageService.GetEnemyPrefabAsync(EnemyType.Enemy1));
        
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
            
            var enemy = GameObject.Instantiate(_prefab);
            enemy.ViewReturner += ReturnToItemsList;
         
            return enemy;
        }
        
        private void ReturnToItemsList(PooledView pooled)
        {
            pooled.gameObject.SetActive(false);
            _freeItems.Add(pooled);
        }
    }
}