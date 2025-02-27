using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class StandingState : IState
    {
        private readonly IStateMachineCharacter _stateMachineCharacter;
        private readonly InputReader _inputReader;
        private readonly AnimationParameters _parameters;
        
        public StandingState(IStateMachineCharacter stateMachineCharacter, AnimationParameters parameters)
        {
            _stateMachineCharacter = stateMachineCharacter;
            _inputReader = stateMachineCharacter.InputReader;
            _parameters = parameters;
        }
        
        public void Enter()
        {
            _stateMachineCharacter.Animator.SetFloat(_parameters.HashVelocityXZ, 0f);
            _stateMachineCharacter.Animator.SetFloat(_parameters.VelocityY, 0f);
        }

        public void Update()
        {
            if (_inputReader.Value != Vector3.zero || _stateMachineCharacter.Controller.velocity.magnitude > 0.01f)
            {
                _stateMachineCharacter.SwitchState<MovingState>();
                return;
            }
            
            if (_inputReader.IsJumping)
                _stateMachineCharacter.SwitchState<JumpingState>();
        }

        public void Exit()
        {
        }
    }
}