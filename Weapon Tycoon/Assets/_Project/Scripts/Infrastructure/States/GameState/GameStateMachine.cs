using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.States.GameState
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)] 
    public class GameStateMachine : IStateSwitcher, IInitializable
    {
        private readonly StateFactory _stateFactory;
        private Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(StateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }
        
        public void Initialize()
        {
            Debug.Log("State machine is initialized");
            
            _states = new()
            {
                { typeof(BootstrapState), _stateFactory.CreateState<BootstrapState>() },
                { typeof(AssetsLoadingState), _stateFactory.CreateState<AssetsLoadingState>() },
                { typeof(GameplayState), _stateFactory.CreateState<GameplayState>() },
            };
            
            SwitchState<BootstrapState>();
        }

        public void SwitchState<T>()
            where T : IState
        {
            var type = typeof(T);

            _currentState?.Exit();
            _currentState = _states[type];
            _currentState?.Enter();
        }
    }
}