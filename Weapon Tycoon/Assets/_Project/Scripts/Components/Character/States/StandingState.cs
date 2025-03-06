using System;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    [Serializable]
    public class StandingState : IState
    {
        private readonly IStateMachineCharacter _stateMachineCharacter;
        private readonly InputReader _inputReader;
        private readonly CharacterController _controller;
        private readonly AnimationParameters _parameters;
        private readonly Camera _camera;
        
        public StandingState(IStateMachineCharacter stateMachineCharacter, AnimationParameters parameters, Camera camera)
        {
            _stateMachineCharacter = stateMachineCharacter;
            _inputReader = stateMachineCharacter.InputReader;
            _controller = _stateMachineCharacter.Controller;
            _parameters = parameters;
            _camera = camera;
        }
        
        public void Enter()
        {
            _stateMachineCharacter.Animator.SetFloat(_parameters.HashVelocityX, 0f);
            _stateMachineCharacter.Animator.SetFloat(_parameters.HashVelocityZ, 0f);
            _stateMachineCharacter.Animator.SetFloat(_parameters.HashMagnitudeXZ, 0f);
            _stateMachineCharacter.Animator.SetFloat(_parameters.HashVelocityY, 0f);
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

            HandleRotation();
        }

        private void HandleRotation()
        {
            if (Input.GetMouseButton(0))
            {
                Quaternion look = Quaternion.LookRotation(-_camera.transform.right, Vector3.up);
                //Quaternion rotation = Quaternion.Euler(0, forwardOnPlane.y, 0);
                Vector3 euler = look.eulerAngles;
                euler.x = 0f;
                euler.z = 0f;
                look = Quaternion.Euler(euler);
                _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, look, 0.15f);
            }
        }

        public void Exit()
        {
        }
    }
}