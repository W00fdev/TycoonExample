using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Components.Character
{
    [Serializable]
    public class StandingState : State
    {
        private readonly IStateMachine _stateMachine;
        private readonly InputReader _inputReader;
        private readonly AnimationParameters _parameters;
        
        public StandingState(IStateMachine stateMachine, InputReader inputReader,
            AnimationParameters parameters)
        {
            _stateMachine = stateMachine;
            _inputReader = inputReader;
            _parameters = parameters;
        }
        
        public override void Enter()
        {
            _stateMachine.Animator.SetFloat(_parameters.HashVelocityXZ, 0f);
            _stateMachine.Animator.SetFloat(_parameters.VelocityY, 0f);
        }

        public override void Update()
        {
            if (_stateMachine.Controller.velocity.magnitude > 0.01f)
            {
                _stateMachine.SwitchState<MovingState>();
                return;
            }    
            
            if (_inputReader.Value != Vector3.zero)
            {
                _stateMachine.SwitchState<MovingState>();
                return;
            }
            
            if (_inputReader.IsJumping)
                _stateMachine.SwitchState<JumpingState>();
        }

        public override void Exit()
        {
        }
    }

    [Serializable]
    public class MovingState : State
    {
        protected readonly IStateMachine _stateMachine;
        protected readonly InputReader _inputReader;
        private readonly Camera _mainCamera;
        
        protected readonly AnimationParameters _parameters;
        protected readonly MovementStats _stats;
        protected Vector3 _velocityY;

        public MovingState(IStateMachine stateMachine, InputReader inputReader,
             Camera mainCamera, MovementStats stats, AnimationParameters parameters)
        {
            _stateMachine = stateMachine;
            _inputReader = inputReader;
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
            
            _stateMachine.Animator.SetFloat(_parameters.HashVelocityXZ, (right + forward).magnitude);
        }
        
        protected void HandleFalling()
        {
            _velocityY += _stats.GravityForce * (_stats.GravityForce.magnitude * 0.5f * Time.deltaTime);

            var controller = _stateMachine.Controller;
            controller.Move(_velocityY * Time.deltaTime);
            
            if (_stateMachine.IsGrounded)
            {
                _stateMachine.Animator.SetFloat(_parameters.HashVelocityY, 0f);
                _velocityY = _stats.GravityForce;
            }
            else
            {
                _stateMachine.Animator.SetFloat(_parameters.HashVelocityY, _velocityY.magnitude);
            }
        }

        public override void Exit()
        {
        }
    }
    
    [Serializable]
    public class JumpingState : MovingState
    {
        private readonly float _jumpTime = 0.85f;
        
        public JumpingState(IStateMachine stateMachine, InputReader inputReader, 
            Camera mainCamera, MovementStats stats, AnimationParameters parameters)
            : base(stateMachine, inputReader, mainCamera, stats, parameters)
        {
        }
        
        public override void Enter()
        {
            _velocityY = _stats.JumpForce;
            _stateMachine.Controller.Move(_velocityY * Time.deltaTime);
            
            /*if (_stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") == false)
                _stateMachine.Animator.SetTrigger(Parameters.JumpTrigger);*/
            
            Debug.Log("Entered JUMP");
        }

        public override void Update()
        {
            HandleMovement();
            HandleFalling();
            
            // Next frame delayed check
            if (_stateMachine.IsGrounded)
            {
                if (_inputReader.Value == Vector3.zero)
                    _stateMachine.SwitchState<StandingState>();
                else
                    _stateMachine.SwitchState<MovingState>();
            }
        }

        public override void Exit()
        {
        }
    }

    /// <summary>
    /// Standing = Idle
    /// Moving = Any Movement
    /// Jumping = Up Impulse
    /// Причем:
    ///     - падение проигрывается, пока не приземлимся (grounded)
    ///     - во время падения и прыжка можно передвигаться (анимация не смешивается)
    ///
    /// Резюмируя:
    ///     + Единственный вариант перехода из Jump в не Jump -> флаг isGrounded
    ///     + Movement -> отдельный стейт, но разделяет логику передвижения с Jumping
    ///     + Standing -> просто стоит
    ///         + Причем проверка jumping важнее movement, но разделяют они общ. логику 
    ///         + --- логика перемещения
    ///         +   --- наследуемая логика прыжка (
    ///         + --- проверка

    ///     + Из movement при прыжка переходим в JumpingState, который дочерний и наследует HandleMovement 


    /// </summary>

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

        private Dictionary<Type, State> _movementStates;
        private InputReader _inputReader;
        private State _currentState;

        public Animator Animator => _animator;
        public CharacterController Controller => _controller;
        
        private void Awake()
        {
            _controller = GetComponentInChildren<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            
            _inputReader = new InputReader();
            _currentState = new StandingState(this, _inputReader, _parameters);
            _movementStates = new Dictionary<Type, State>()
            {
                { typeof(StandingState), _currentState },
                { typeof(MovingState), new MovingState(this, _inputReader, _mainCamera, _stats, _parameters) },
                { typeof(JumpingState), new JumpingState(this, _inputReader, _mainCamera, _stats, _parameters) },
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
            where T : State
        {
            var type = typeof(T);
            
            _currentState?.Exit();
            _movementStates.TryGetValue(type, out var state);
            _currentState = state;
            
            state?.Enter();
        }
    }
}