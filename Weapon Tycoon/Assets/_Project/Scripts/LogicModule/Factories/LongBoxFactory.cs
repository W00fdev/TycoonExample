using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.LogicModule.Factories
{
    public class LongBoxFactory : BoxFactory
    {
        protected LongBoxFactory(PooledView prefab) : base(prefab) { }
        
        public static async UniTask<BoxFactory> CreateAsync(StorageService storageService) 
            => new LongBoxFactory(await storageService.GetBoxViewAsync(BoxType.LongBox));
    }
}