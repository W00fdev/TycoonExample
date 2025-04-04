using System;
using System.Collections.Generic;
using _Project.Scripts.Components.Character.States;
using _Project.Scripts.Infrastructure.States;
using UnityEngine;
using Zenject;

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
        public string VelocityY;
        public string VelocityX;
        public string VelocityZ;
        public string MagnitudeXZ;

        public int HashMagnitudeXZ => Animator.StringToHash(MagnitudeXZ);
        public int HashVelocityX => Animator.StringToHash(VelocityX);
        public int HashVelocityZ => Animator.StringToHash(VelocityZ);
        public int HashVelocityY=> Animator.StringToHash(VelocityY);
    }
    
    public class PlayerMovement : MonoBehaviour, IInitializable, ITickable, ICharacterStateMachine
    {
        [SerializeField] private AnimationParameters _parameters;
        [SerializeField] private MovementStats _stats;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;
        
        private Animator _animator;
        private CharacterController _controller;

        private Dictionary<Type, ITickableState> _movementStates;
        private InputReader _inputReader;
        private ITickableState _currentTickableState;

        public Animator Animator => _animator;
        public CharacterController Controller => _controller;

        [Inject]
        public void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
        }
        
        public void Initialize()
        {
            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<CharacterController>();

            _currentTickableState = new StandingTickableState(this, _parameters, _inputReader, _mainCamera);
            _movementStates = new Dictionary<Type, ITickableState>()
            {
                { typeof(StandingTickableState), _currentTickableState },
                { typeof(MovingTickableState), new MovingTickableState(this, _mainCamera, _stats, _parameters, _inputReader) },
                { typeof(JumpingTickableState), new JumpingTickableState(this, _mainCamera, _stats, _parameters, _inputReader) },
            };
        }

        public void Tick() => _currentTickableState.Tick();

        public bool IsGrounded
            => Controller.isGrounded; /*|| Physics.CheckSphere(_groundCheck.position, 0.0001f, _groundLayer.value);*/

        public void SwitchState<T>()
            where T : IState
        {
            var type = typeof(T);
            
            _currentTickableState?.Exit();
            _movementStates.TryGetValue(type, out var state);
            _currentTickableState = state;
            
            state?.Enter();
        }
    }
}