using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure;

namespace _Project.Scripts.LogicModule.Factories
{
    public class LongBoxFactory : BoxFactory
    {
        public LongBoxFactory(StorageService service) : base(service)
        {
            StorageService.GetBoxView(BoxType.LongBox, (x) => _prefab = x);
        }
    }
}