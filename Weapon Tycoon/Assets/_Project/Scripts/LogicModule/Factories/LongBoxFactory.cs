using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;

namespace _Project.Scripts.LogicModule.Factories
{
    public class LongBoxFactory : BoxFactory
    {
        public LongBoxFactory(StorageService storageService) : base(storageService)
        {
            _prefab = _storageService.GetBoxView(BoxStorage.BoxType.LongBox);
        }
    }
}