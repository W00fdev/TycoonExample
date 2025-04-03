using _Project.Scripts.Infrastructure.Bootstrappers;
using _Project.Scripts.Infrastructure.States.GameState;
using _Project.Scripts.LogicModule;
using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    [SerializeField] private UIBootstrap _uiBootstrap;
    [SerializeField] private EconomyShop _economyShop;
    [SerializeField] private DefenseShop _defenseShop;
    
    public override void InstallBindings()
    {
        Container
            .Bind<UIBootstrap>()
            .FromInstance(_uiBootstrap)
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<EconomyShop>()
            .FromInstance(_economyShop)
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<DefenseShop>()
            .FromInstance(_defenseShop)
            .AsSingle()
            .NonLazy();
     
        Container
            .Bind<BootstrapState>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<AssetsLoadingState>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<GameplayState>()
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<StateFactory>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<GameStateMachine>()
            .AsSingle()
            .NonLazy();
    }
}