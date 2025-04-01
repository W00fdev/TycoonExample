using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Components.Enemies;
using _Project.Scripts.Components.Enemies.States;
using _Project.Scripts.Infrastructure.Data.Enemies;
using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using _Project.Scripts.Infrastructure.States;
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
        
        private Dictionary<Type, ITickableState> _states;
        private ITickableState _currentTickableState;

        private Transform _target;

        public Animator Animator => _animator;
        public NavMeshAgent Agent => _agent;
        public Transform Target => _target;

        public void Initialize(Transform target)
        {
            if (_currentTickableState == null)
                CreateStates();

            _target = target;
            _currentTickableState = _states[typeof(WalkingTickableState)];
            _currentTickableState.Enter();
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

        public void SwitchState<T>() where T : ITickableState
        {
            var type = typeof(T);
            
            _currentTickableState?.Exit();
            _states.TryGetValue(type, out var state);
            _currentTickableState = state;
            
            state?.Enter();
        }

        private void CreateStates()
        {
            _states = new Dictionary<Type, ITickableState>()
            {
                {typeof(WalkingTickableState), new WalkingTickableState(this, _config) },
                {typeof(MeleeAttackTickableState), new MeleeAttackTickableState(this, _targetLayer, _config) },
                {typeof(DyingTickableState), new DyingTickableState(this, ReturnToPool) },
            };
        }
        
        private void EnterDeathState() => SwitchState<DyingTickableState>();

        private void AnimateDamage() => Tween.PunchScale(_basicModel, Vector3.up * 0.1f, 0.1f);

        private void Reward() => _addMoneychannel.Invoke(_config.Data.Reward);
    }
}