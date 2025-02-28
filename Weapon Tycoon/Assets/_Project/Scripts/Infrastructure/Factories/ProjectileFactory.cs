using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Factories
{
    [Serializable]
    public class ProjectileFactory : CustomPool
    {
        protected PooledView _prefab;
        
        protected ProjectileFactory(PooledView prefab)
            => _prefab = prefab;
        
        public static async UniTask<ProjectileFactory> CreateAsync(StorageService storageService) 
            => new ProjectileFactory(await storageService.GetProjectileViewAsync(ProjectileType.LaserYellow));

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