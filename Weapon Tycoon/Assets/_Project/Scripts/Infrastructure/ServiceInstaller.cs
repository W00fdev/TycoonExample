using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Factories.Accessors;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StorageService>().AsSingle();
            Container.Bind<PersistentProgress>().AsSingle();

            Container.Bind<PistolFactoryAccessor<PistolFactory>>().AsSingle();
            Container.Bind<ShotgunFactoryAccessor<ShotgunFactory>>().AsSingle();
            Container.Bind<RifleFactoryAccessor<RifleFactory>>().AsSingle();
            Container.Bind<BoxFactoryAccessor<BoxFactory>>().AsSingle();
            Container.Bind<BoxFactoryAccessor<LongBoxFactory>>().AsSingle();
            Container.Bind<ProjectileFactoryAccessor<ProjectileFactory>>().AsSingle();
            Container.Bind<ProjectileFactoryAccessor<ExplosionFactory>>().AsSingle();
            Container.Bind<MoneyTextFactoryAccessor>().AsSingle();
        }
    }
}