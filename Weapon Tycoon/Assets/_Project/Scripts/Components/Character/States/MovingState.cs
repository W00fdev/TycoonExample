using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class MovingState : IState
    {
        protected readonly IStateMachineCharacter StateMachineCharacter;
        protected readonly InputReader _inputReader;
        private readonly Camera _mainCamera;
        
        protected readonly AnimationParameters _parameters;
        protected readonly MovementStats _stats;
        protected Vector3 _velocityY;

        public MovingState(IStateMachineCharacter stateMachineCharacter, Camera mainCamera,
            MovementStats stats, AnimationParameters parameters)
        {
            StateMachineCharacter = stateMachineCharacter;
            _inputReader = StateMachineCharacter.InputReader;
            _mainCamera = mainCamera;

            _velocityY = Vector3.zero;
            _stats = stats;
            _parameters = parameters;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
            HandleMovement();
            HandleFalling();
            
            if (_inputReader.Value == Vector3.zero && StateMachineCharacter.Controller.velocity.magnitude < 0.001f)
            {
                StateMachineCharacter.SwitchState<StandingState>();
                return;
            }

            if (_inputReader.IsJumping && StateMachineCharacter.IsGrounded)
                StateMachineCharacter.SwitchState<JumpingState>();
        }

        protected void HandleMovement()
        {
            Vector3 forward = _mainCamera.transform.forward;
            Vector3 right = _mainCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward = forward.normalized * (_inputReader.Value.z * Time.deltaTime);
            right = right.normalized * (_inputReader.Value.x * Time.deltaTime);
            
            float angle = Mathf.Atan2(-forward.z - right.z, forward.x + right.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            var controller = StateMachineCharacter.Controller;
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rotation, 0.15f);
            controller.Move((right + forward) * _stats.Speed);
            
            SetAnimationVelocityXZ((right + forward).magnitude);
        }
        
        protected virtual void HandleFalling()
        {
            _velocityY += _stats.GravityForce * (_stats.GravityForce.magnitude * 0.5f * Time.deltaTime);

            var controller = StateMachineCharacter.Controller;
            controller.Move(_velocityY * Time.deltaTime);
            
            if (StateMachineCharacter.IsGrounded)
            {
                ResetAnimationVelocityY();
                _velocityY = _stats.GravityForce;
            }
        }

        private void SetAnimationVelocityXZ(float xzMagnitude) 
            => StateMachineCharacter.Animator.SetFloat(_parameters.HashVelocityXZ, xzMagnitude);
        
        private void ResetAnimationVelocityY() => StateMachineCharacter.Animator.SetFloat(_parameters.HashVelocityY, 0f);

        public void Exit()
        {
        }
    }
}