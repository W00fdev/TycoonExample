using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class MovingState : State
    {
        protected readonly IStateMachine _stateMachine;
        protected readonly InputReader _inputReader;
        private readonly Camera _mainCamera;
        
        protected readonly AnimationParameters _parameters;
        protected readonly MovementStats _stats;
        protected Vector3 _velocityY;

        public MovingState(IStateMachine stateMachine, Camera mainCamera,
            MovementStats stats, AnimationParameters parameters)
        {
            _stateMachine = stateMachine;
            _inputReader = _stateMachine.InputReader;
            _mainCamera = mainCamera;

            _velocityY = Vector3.zero;
            _stats = stats;
            _parameters = parameters;
        }

        public override void Enter()
        {
        }

        public override void Update()
        {
            HandleMovement();
            HandleFalling();
            
            if (_inputReader.Value == Vector3.zero)
            {
                _stateMachine.SwitchState<StandingState>();
                return;
            }

            if (_inputReader.IsJumping && _stateMachine.IsGrounded)
                _stateMachine.SwitchState<JumpingState>();
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

            var controller = _stateMachine.Controller;
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rotation, 0.15f);
            controller.Move((right + forward) * _stats.Speed);
            
            SetAnimationVelocityXZ((right + forward).magnitude);
        }
        
        protected virtual void HandleFalling()
        {
            _velocityY += _stats.GravityForce * (_stats.GravityForce.magnitude * 0.5f * Time.deltaTime);

            var controller = _stateMachine.Controller;
            controller.Move(_velocityY * Time.deltaTime);
            
            if (_stateMachine.IsGrounded)
            {
                SetAnimationVelocityY();
                _velocityY = _stats.GravityForce;
            }
        }

        private void SetAnimationVelocityXZ(float xzMagnitude) 
            => _stateMachine.Animator.SetFloat(_parameters.HashVelocityXZ, xzMagnitude);
        
        private void SetAnimationVelocityY() => _stateMachine.Animator.SetFloat(_parameters.HashVelocityY, 0f);

        public override void Exit()
        {
        }
    }
}