using System;
using System.Collections.Generic;
using _Project.Scripts.Components.Character;

namespace _Project.Scripts.Infrastructure.States.GameState
{
    public class GameStateMachine : IStateSwitcher
    {
        private readonly Dictionary<Type, IState> _movementStates;
        private IState _currentState;

        public GameStateMachine()
        {
            _movementStates = new()
            {
                {typeof(BootstrapState), new BootstrapState(this)},
                {typeof(AssetsLoadingState), new AssetsLoadingState(this)},
                {typeof(GameplayState), new GameplayState(this)},
            };
        }
        
        public void SwitchState<T>() where T : ITickableState
        {
            var type = typeof(T);
            
            _currentState?.Exit();
            _currentState = _movementStates[type];
            _currentState?.Enter();
        }
    }
}