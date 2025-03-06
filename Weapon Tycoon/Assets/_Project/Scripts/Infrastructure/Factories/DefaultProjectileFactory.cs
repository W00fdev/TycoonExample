using System;
using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Factories
{
    [Serializable]
    public class DefaultProjectileFactory : ProjectileFactory
    {
        protected DefaultProjectileFactory(PooledView prefab) : base(prefab)
            => _prefab = prefab;
        
        public new static async UniTask<ProjectileFactory> CreateAsync(StorageService storageService) 
            => new DefaultProjectileFactory(await storageService.GetProjectileViewAsync(ProjectileType.Projectile));
    }
}