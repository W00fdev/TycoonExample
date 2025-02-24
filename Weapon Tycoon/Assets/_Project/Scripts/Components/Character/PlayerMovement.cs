using System;
using System.Collections.Generic;
using _Project.Scripts.Components.Character.States;
using UnityEngine;

namespace _Project.Scripts.Components.Character
{
    [Serializable]
    public struct MovementStats
    {
        public float Speed;
        public Vector3 JumpForce;
        public Vector3 GravityForce;
    }
    
    [Serializable]
    public struct AnimationParameters
    {
        public string VelocityXZ;
        public string VelocityY;

        public int HashVelocityXZ => Animator.StringToHash(VelocityXZ);
        public int HashVelocityY=> Animator.StringToHash(VelocityY);
    }
    
    public class PlayerMovement : MonoBehaviour, IStateMachine
    {
        [SerializeField] private AnimationParameters _parameters;
        [SerializeField] private MovementStats _stats;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;
        
        private Animator _animator;
        private CharacterController _controller;

        private Dictionary<Type, IState> _movementStates;
        private InputReader _inputReader;
        private IState _currentState;

        public Animator Animator => _animator;
        public CharacterController Controller => _controller;
        public InputReader InputReader => _inputReader;
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<CharacterController>();
            
            _inputReader = new InputReader();
            _currentState = new StandingState(this, _parameters);
            _movementStates = new Dictionary<Type, IState>()
            {
                { typeof(StandingState), _currentState },
                { typeof(MovingState), new MovingState(this, _mainCamera, _stats, _parameters) },
                { typeof(JumpingState), new JumpingState(this, _mainCamera, _stats, _parameters) },
            };
        }

        private void Update()
        {
            _inputReader.Update();
            _currentState.Update();
        }

        public bool IsGrounded
            => Controller.isGrounded; /*|| Physics.CheckSphere(_groundCheck.position, 0.0001f, _groundLayer.value);*/

        public void SwitchState<T>()
            where T : IState
        {
            var type = typeof(T);
            
            _currentState?.Exit();
            _movementStates.TryGetValue(type, out var state);
            _currentState = state;
            
            state?.Enter();
        }
    }
}