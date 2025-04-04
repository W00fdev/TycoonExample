using System;
using _Project.Scripts.Infrastructure.States;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class MovingTickableState : ITickableState
    {
        protected readonly ICharacterStateMachine CharacterStateMachine;
        protected readonly InputReader _inputReader;
        protected readonly CharacterController _controller;
        protected readonly Animator _animator;
        private readonly Camera _mainCamera;
        
        protected readonly AnimationParameters _parameters;
        protected readonly MovementStats _stats;
        protected Vector3 _velocityY;

        public MovingTickableState(ICharacterStateMachine characterStateMachine, Camera mainCamera,
            MovementStats stats, AnimationParameters parameters, InputReader inputReader)
        {
            CharacterStateMachine = characterStateMachine;
            _controller = CharacterStateMachine.Controller;
            _animator = CharacterStateMachine.Animator;
            _inputReader = inputReader;
            _mainCamera = mainCamera;

            _velocityY = Vector3.zero;
            _stats = stats;
            _parameters = parameters;
        }

        public virtual void Enter()
        {
        }

        public virtual void Tick()
        {
            HandleMovement();
            HandleFalling();
            
            if (_inputReader.Value == Vector3.zero && _controller.velocity.magnitude < 0.001f)
            {
                CharacterStateMachine.SwitchState<StandingTickableState>();
                return;
            }

            if (_inputReader.IsJumping && _controller.isGrounded)
                CharacterStateMachine.SwitchState<JumpingTickableState>();
        }

        protected void HandleMovement()
        {
            Vector3 forward = _mainCamera.transform.forward;
            Vector3 right = _mainCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward = forward.normalized * (_inputReader.Value.z * Time.deltaTime);
            right = right.normalized * (_inputReader.Value.x * Time.deltaTime);
            
            Quaternion look = Quaternion.LookRotation(-_mainCamera.transform.right, Vector3.up);
            Vector3 euler = Vector3.up * look.eulerAngles.y;
            look = Quaternion.Euler(euler);
            
            _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, look, 0.15f);
            _controller.Move((right + forward) * _stats.Speed);
            
            SetAnimationVelocityXZ(_inputReader.Value.x, _inputReader.Value.z);
        }
        
        protected virtual void HandleFalling()
        {
            _velocityY += _stats.GravityForce * (_stats.GravityForce.magnitude * 0.5f * Time.deltaTime);
            _controller.Move(_velocityY * Time.deltaTime);
            
            if (_controller.isGrounded)
            {
                ResetAnimationVelocityY();
                _velocityY = _stats.GravityForce;
            }
        }

        private void SetAnimationVelocityXZ(float xMagnitude, float zMagnitude)
        {
            _animator.SetFloat(_parameters.VelocityX, xMagnitude);
            _animator.SetFloat(_parameters.VelocityZ, zMagnitude);
            
            // sqrt(x^2 + y^2) ~= (x+y) * sqrt(2)
            _animator.SetFloat(_parameters.MagnitudeXZ, (Math.Abs(xMagnitude) + Math.Abs(zMagnitude)) * 0.7f);
        }

        private void ResetAnimationVelocityY() => _animator.SetFloat(_parameters.HashVelocityY, 0f);

        public void Exit()
        {
        }
    }
}