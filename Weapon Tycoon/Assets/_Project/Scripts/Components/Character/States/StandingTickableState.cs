using System;
using _Project.Scripts.Infrastructure.States;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class StandingTickableState : ITickableState
    {
        private readonly ICharacterStateMachine _characterStateMachine;
        private readonly InputReader _inputReader;
        private readonly CharacterController _controller;
        private readonly AnimationParameters _parameters;
        private readonly Camera _camera;
        
        public StandingTickableState(ICharacterStateMachine characterStateMachine, AnimationParameters parameters, InputReader inputReader, Camera camera)
        {
            _characterStateMachine = characterStateMachine;
            _inputReader = inputReader;
            _controller = _characterStateMachine.Controller;
            _parameters = parameters;
            _camera = camera;
        }
        
        public void Enter()
        {
            _characterStateMachine.Animator.SetFloat(_parameters.HashVelocityX, 0f);
            _characterStateMachine.Animator.SetFloat(_parameters.HashVelocityZ, 0f);
            _characterStateMachine.Animator.SetFloat(_parameters.HashMagnitudeXZ, 0f);
            _characterStateMachine.Animator.SetFloat(_parameters.HashVelocityY, 0f);
        }

        public void Tick()
        {
            if (_inputReader.Value != Vector3.zero || _characterStateMachine.Controller.velocity.magnitude > 0.01f)
            {
                _characterStateMachine.SwitchState<MovingTickableState>();
                return;
            }
            
            if (_inputReader.IsJumping)
                _characterStateMachine.SwitchState<JumpingTickableState>();

            HandleRotation();
        }

        private void HandleRotation()
        {
            if (_inputReader.IsLeftMouseButton == false)
                return;
            
            Quaternion look = Quaternion.LookRotation(-_camera.transform.right, Vector3.up);
            //Quaternion rotation = Quaternion.Euler(0, forwardOnPlane.y, 0);
            Vector3 euler = look.eulerAngles;
            euler.x = 0f;
            euler.z = 0f;
            look = Quaternion.Euler(euler);
            _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, look, 0.15f);
        }

        public void Exit()
        {
        }
    }
}