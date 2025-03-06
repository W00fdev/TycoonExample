using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Components.Enemies;
using _Project.Scripts.Components.Enemies.States;
using _Project.Scripts.Infrastructure.Data.Enemies;
using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using _Project.Scripts.LogicModule.Views;
using PrimeTween;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Components
{
    public class Enemy : PooledView, IStateMachineEnemy
    {
        [SerializeField] private Transform _basicModel;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private CurrencyEventChannel _addMoneychannel;
        [SerializeField] private Health _health;
        
        private Dictionary<Type, IState> _states;
        private IState _currentState;

        private Transform _target;

        public Animator Animator => _animator;
        public NavMeshAgent Agent => _agent;
        public Transform Target => _target;

        public void Initialize(Transform target)
        {
            if (_currentState == null)
                CreateStates();

            _target = target;
            _currentState = _states[typeof(WalkingState)];
            _currentState.Enter();
        }

        private void Awake()
        {
            ViewReturner += (_) => Reward();
        }

        private void OnEnable()
        {
            _health.Initialize(_config.Data.Health);
            
            _health.DamagedEvent += AnimateDamage;
            _health.DiedEvent += EnterDeathState;
        }

        private void OnDisable()
        {
            _health.DamagedEvent -= AnimateDamage;
            _health.DiedEvent -= EnterDeathState;
        }

        private void OnDestroy()
        {
            ViewReturner -= (_) => Reward();
        }

        public void SwitchState<T>() where T : IState
        {
            var type = typeof(T);
            
            _currentState?.Exit();
            _states.TryGetValue(type, out var state);
            _currentState = state;
            
            state?.Enter();
        }

        private void CreateStates()
        {
            _states = new Dictionary<Type, IState>()
            {
                {typeof(WalkingState), new WalkingState(this, _config) },
                {typeof(MeleeAttackState), new MeleeAttackState(this, _targetLayer, _config) },
                {typeof(DyingState), new DyingState(this, ReturnToPool) },
            };
        }
        
        private void EnterDeathState() => SwitchState<DyingState>();

        private void AnimateDamage() => Tween.PunchScale(_basicModel, Vector3.up * 0.1f, 0.1f);

        private void Reward() => _addMoneychannel.Invoke(_config.Data.Reward);
    }
}