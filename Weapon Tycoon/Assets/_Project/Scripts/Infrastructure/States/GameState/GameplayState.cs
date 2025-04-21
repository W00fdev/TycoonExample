using System;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Infrastructure.Bootstrappers;
using _Project.Scripts.LogicModule;
using _Project.Scripts.Utils;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.States.GameState
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)] 
    public class GameplayState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly EconomyShop _economyShop;
        private readonly DefenseShop _defenseShop;
        private readonly UIBootstrap _uiBootstrap;
        private readonly WeaponHolder _weaponHolder;

        public GameplayState(GameStateMachine stateSwitcher, EconomyShop economyShop,
            DefenseShop defenseShop, UIBootstrap uiBootstrap, WeaponHolder weaponHolder)
        {
            _stateSwitcher = stateSwitcher;
            _economyShop = economyShop;
            _defenseShop = defenseShop;
            _uiBootstrap = uiBootstrap;
            _weaponHolder = weaponHolder;
        }
        
        public void Enter()
        {
            _uiBootstrap.Initialize();
            _economyShop.Initialize();
            _defenseShop.Initialize();
            
            _weaponHolder.Initialize();
        }

        public void Exit()
        {
        }
    }
}