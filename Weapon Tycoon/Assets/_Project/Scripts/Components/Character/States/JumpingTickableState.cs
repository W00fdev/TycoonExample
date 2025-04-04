using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class JumpingTickableState : MovingTickableState
    {
        public JumpingTickableState(ICharacterStateMachine characterStateMachine, Camera mainCamera,
            MovementStats stats, AnimationParameters parameters, InputReader inputReader)
            : base(characterStateMachine, mainCamera, stats, parameters, inputReader)
        {
        }
        
        public override void Enter()
        {
            _velocityY = _stats.JumpForce;
            _controller.Move(_velocityY * Time.deltaTime);
        }

        public override void Tick()
        {
            HandleMovement();
            HandleFalling();

            if (!_controller.isGrounded) 
                return;
            
            if (_inputReader.Value == Vector3.zero)
                CharacterStateMachine.SwitchState<StandingTickableState>();
            else
                CharacterStateMachine.SwitchState<MovingTickableState>();
        }

        protected override void HandleFalling()
        {
            base.HandleFalling();
            
            if (_controller.isGrounded == false)
                _animator.SetFloat(_parameters.HashVelocityY, _velocityY.magnitude);
        }
    }
}