using System;
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

        public GameplayState(GameStateMachine stateSwitcher, EconomyShop economyShop,
            DefenseShop defenseShop, UIBootstrap uiBootstrap)
        {
            _stateSwitcher = stateSwitcher;
            _economyShop = economyShop;
            _defenseShop = defenseShop;
            _uiBootstrap = uiBootstrap;
        }
        
        public void Enter()
        {
            _uiBootstrap.Initialize();
            _economyShop.Initialize();
            _defenseShop.Initialize();
        }

        public void Exit()
        {
        }
    }
}