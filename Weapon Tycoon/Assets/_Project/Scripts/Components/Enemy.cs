using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Components.Enemies;
using _Project.Scripts.Components.Enemies.States;
using PrimeTween;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Components
{
    public class Enemy : MonoBehaviour, IStateMachineEnemy
    {
        [SerializeField] private Transform _basicModel;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private int _damage;
        
        private Dictionary<Type, IState> _states;
        private IState _currentState;
        private Health _health;

        private Transform _target;

        public Animator Animator => _animator;
        public NavMeshAgent Agent => _agent;
        public Transform Target => _target;

        private void Awake()
        {
            
        }

        public void Initialize(Transform target)
        {
            // config, wall, flag with case

            if (_currentState == null)
                CreateStates();

            _target = target;
            _currentState = _states[typeof(WalkingState)];
            _currentState.Enter();
        }

        private void CreateStates()
        {
            _states = new Dictionary<Type, IState>()
            {
                {typeof(WalkingState), new WalkingState(this) },
                {typeof(MeleeAttackState), new MeleeAttackState(this, _targetLayer, _damage) },
                {typeof(DyingState), new DyingState(_animator, ReturnToPool) },
            };
        }

        private void OnEnable()
        {
            _health.DamagedEvent += AnimateDamage;
            _health.DiedEvent += EnterDeathState;
        }

        private void OnDisable()
        {
            _health.DamagedEvent -= AnimateDamage;
            _health.DiedEvent -= EnterDeathState;
        }
        
        public void SwitchState<T>() where T : IState
        {
            var type = typeof(T);
            
            _currentState?.Exit();
            _states.TryGetValue(type, out var state);
            _currentState = state;
            
            state?.Enter();
        }

        private void EnterDeathState() => SwitchState<DyingState>();

        private void AnimateDamage() => Tween.PunchScale(_basicModel, Vector3.up * 0.1f, 0.1f);

        private void ReturnToPool() => GameObject.Destroy(gameObject);
    }
}