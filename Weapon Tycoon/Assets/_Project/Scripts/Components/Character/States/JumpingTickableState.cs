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
            CharacterStateMachine.Controller.Move(_velocityY * Time.deltaTime);
        }

        public override void Tick()
        {
            HandleMovement();
            HandleFalling();

            if (!CharacterStateMachine.IsGrounded) 
                return;
            
            if (_inputReader.Value == Vector3.zero)
                CharacterStateMachine.SwitchState<StandingTickableState>();
            else
                CharacterStateMachine.SwitchState<MovingTickableState>();
        }

        protected override void HandleFalling()
        {
            base.HandleFalling();
            
            if (CharacterStateMachine.IsGrounded == false)
                CharacterStateMachine.Animator.SetFloat(_parameters.HashVelocityY, _velocityY.magnitude);
        }
    }
}