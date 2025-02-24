using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class JumpingState : MovingState
    {
        public JumpingState(IStateMachine stateMachine, Camera mainCamera,
            MovementStats stats, AnimationParameters parameters)
            : base(stateMachine, mainCamera, stats, parameters)
        {
        }
        
        public override void Enter()
        {
            _velocityY = _stats.JumpForce;
            _stateMachine.Controller.Move(_velocityY * Time.deltaTime);
        }

        public override void Update()
        {
            HandleMovement();
            HandleFalling();

            if (!_stateMachine.IsGrounded) 
                return;
            
            if (_inputReader.Value == Vector3.zero)
                _stateMachine.SwitchState<StandingState>();
            else
                _stateMachine.SwitchState<MovingState>();
        }

        protected override void HandleFalling()
        {
            base.HandleFalling();
            
            if (_stateMachine.IsGrounded == false)
                _stateMachine.Animator.SetFloat(_parameters.HashVelocityY, _velocityY.magnitude);
        }

        public override void Exit()
        {
        }
    }
}