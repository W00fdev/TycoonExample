using _Project.Scripts.Components;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure.Bootstrappers;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.States.GameState;
using _Project.Scripts.LogicModule;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private UIBootstrap _uiBootstrap;
        [SerializeField] private EconomyShop _economyShop;
        [SerializeField] private DefenseShop _defenseShop;

        //[SerializeField] private Enemy.EnemySceneReferences _enemySceneReferences;
        [SerializeField] private WeaponHolder _weaponHolder;
        [SerializeField] private PlayerMovement _playerMovement;
        
        [SerializeField] private Enemy.EnemySceneReferences _enemySceneReferences;
        
        public override void InstallBindings()
        {
            InstallSceneReferences();
            InstallGameStateMachine();
            InstallSceneFactories();
            InstallPlayer();
        }

        private void InstallPlayer()
        {
            // Bind all except IStateMachineCharacter
            Container
                .Bind(typeof(IInitializable), typeof(ITickable))
                .FromInstance(_playerMovement)
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<WeaponHolder>()
                .FromInstance(_weaponHolder)
                .AsSingle()
                .NonLazy();
        }

        private void InstallSceneFactories()
        {
            Container
                .Bind<Enemy.EnemySceneReferences>()
                .FromInstance(_enemySceneReferences)
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<EnemyFactory>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallGameStateMachine()
        {
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

        private void InstallSceneReferences()
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
        }
    }
}