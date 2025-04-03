using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Loading;
using _Project.Scripts.Infrastructure.Pools;
using _Project.Scripts.Infrastructure.SaveLoad;
using _Project.Scripts.Infrastructure.Storage;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private Transform _poolRoot;
        [SerializeField] private LoadingCurtainService _loadingCurtain;
        
        public override void InstallBindings()
        {
            Container.Bind<StorageService>().AsSingle();
            Container.Bind<PersistentProgress>().AsSingle();

            Container
                .Bind<ObjectPoolService>()
                .AsSingle()
                .WithArguments(_poolRoot);

            Container
                .Bind<LoadingCurtainService>()
                .FromComponentInNewPrefab(_loadingCurtain)
                .AsSingle()
                .NonLazy();

            ISaveLoadService cacheSaveLoaded = new CacheSaveLoad();
            ISaveLoadService cloudLoaded = new CloudSaveLoad(cacheSaveLoaded);
            Container
                .Bind<ISaveLoadService>()
                .FromInstance(cloudLoaded)
                .AsSingle()
                .NonLazy();
        }
    }
}