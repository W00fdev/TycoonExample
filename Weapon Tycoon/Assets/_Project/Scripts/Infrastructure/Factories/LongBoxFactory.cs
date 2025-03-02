using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Factories
{
    public class LongBoxFactory : BoxFactory
    {
        protected LongBoxFactory(PooledView prefab) : base(prefab) { }
        
        public new static async UniTask<BoxFactory> CreateAsync(StorageService storageService) 
            => new LongBoxFactory(await storageService.GetBoxViewAsync(BoxType.LongBox));
    }
}