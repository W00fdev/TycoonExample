using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class StandingState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly InputReader _inputReader;
        private readonly AnimationParameters _parameters;
        
        public StandingState(IStateMachine stateMachine, AnimationParameters parameters)
        {
            _stateMachine = stateMachine;
            _inputReader = stateMachine.InputReader;
            _parameters = parameters;
        }
        
        public void Enter()
        {
            _stateMachine.Animator.SetFloat(_parameters.HashVelocityXZ, 0f);
            _stateMachine.Animator.SetFloat(_parameters.VelocityY, 0f);
        }

        public void Update()
        {
            if (_inputReader.Value != Vector3.zero || _stateMachine.Controller.velocity.magnitude > 0.01f)
            {
                _stateMachine.SwitchState<MovingState>();
                return;
            }
            
            if (_inputReader.IsJumping)
                _stateMachine.SwitchState<JumpingState>();
        }

        public void Exit()
        {
        }
    }
}