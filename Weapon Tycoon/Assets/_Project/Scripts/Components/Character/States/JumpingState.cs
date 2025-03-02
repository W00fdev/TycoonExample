using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class JumpingState : MovingState
    {
        public JumpingState(IStateMachineCharacter stateMachineCharacter, Camera mainCamera,
            MovementStats stats, AnimationParameters parameters)
            : base(stateMachineCharacter, mainCamera, stats, parameters)
        {
        }
        
        public override void Enter()
        {
            _velocityY = _stats.JumpForce;
            StateMachineCharacter.Controller.Move(_velocityY * Time.deltaTime);
        }

        public override void Update()
        {
            HandleMovement();
            HandleFalling();

            if (!StateMachineCharacter.IsGrounded) 
                return;
            
            if (_inputReader.Value == Vector3.zero)
                StateMachineCharacter.SwitchState<StandingState>();
            else
                StateMachineCharacter.SwitchState<MovingState>();
        }

        protected override void HandleFalling()
        {
            base.HandleFalling();
            
            if (StateMachineCharacter.IsGrounded == false)
                StateMachineCharacter.Animator.SetFloat(_parameters.HashVelocityY, _velocityY.magnitude);
        }
    }
}