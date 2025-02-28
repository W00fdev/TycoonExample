using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Factories
{
    public class ExplosionFactory : ProjectileFactory
    {
        protected ExplosionFactory(PooledView prefab) : base(prefab) { }
        
        public static async UniTask<ExplosionFactory> CreateAsync(StorageService storageService) 
            => new ExplosionFactory(await storageService.GetExplosionViewAsync(ExplosionType.LaserExplosionYellow));
    }
}