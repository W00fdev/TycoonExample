using _Project.Scripts.Infrastructure.Data;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StorageService>().AsSingle();
            Container.Bind<PersistentProgress>().AsSingle();
        }
    }
}