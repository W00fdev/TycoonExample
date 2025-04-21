using System;
using System.Collections.Generic;
using _Project.Scripts.Components.Enemies;
using _Project.Scripts.Components.Enemies.States;
using _Project.Scripts.Infrastructure.Data.Enemies;
using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.LogicModule;
using _Project.Scripts.LogicModule.Views;
using PrimeTween;
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
        [SerializeField] private CurrencyEventChannel _addMoneyChannel;
        [SerializeField] private HealthComponent _healthComponent;
        
        private Dictionary<Type, ITickableState> _states;
        private ITickableState _currentTickableState;

        private EnemySceneReferences _sceneReferences;
        private bool _isInitialized;
        
        public Animator Animator => _animator;
        public NavMeshAgent Agent => _agent;
        public VolumePivot Target => _sceneReferences.WallHealthComponent.IsAlive 
            ? _sceneReferences.WallPivot 
            : _sceneReferences.FlagPivot;

        public bool IsInitialized => _isInitialized;

        [Serializable]
        public class EnemySceneReferences
        {
            public HealthComponent WallHealthComponent;
            public VolumePivot WallPivot;
            public VolumePivot FlagPivot;
        }
        
        public void Initialize(EnemySceneReferences sceneReferences)
        {
            _sceneReferences = sceneReferences;
            
            CreateStates();
        }

        private void Awake()
        {
            ViewReturner += (_) => Reward();
        }

        private void OnEnable()
        {
            _healthComponent.Initialize(_config.Data.Health);
            
            _healthComponent.DamagedEvent += AnimateDamage;
            _healthComponent.DiedEvent += EnterDeathState;
        }

        private void OnDisable()
        {
            _healthComponent.DamagedEvent -= AnimateDamage;
            _healthComponent.DiedEvent -= EnterDeathState;
        }

        private void OnDestroy()
        {
            ViewReturner -= (_) => Reward();
        }

        public void SwitchState<T>() 
            where T : IState
        {
            var type = typeof(T);
            
            _currentTickableState?.Exit();
            _states.TryGetValue(type, out var state);
            _currentTickableState = state;
            
            state?.Enter();
        }

        public void StartStateMachine()
        {
            _currentTickableState = _states[typeof(WalkingEnemyState)];
            _currentTickableState.Enter();
        }
        
        private void CreateStates()
        {
            _states = new Dictionary<Type, ITickableState>()
            {
                {typeof(WalkingEnemyState), new WalkingEnemyState(this, _config) },
                {typeof(MeleeAttackEnemyState), new MeleeAttackEnemyState(this, _targetLayer, _config) },
                {typeof(DyingEnemyState), new DyingEnemyState(this, ReturnToPool) },
            };
        }

        private void EnterDeathState() => SwitchState<DyingEnemyState>();

        private void AnimateDamage() => Tween.PunchScale(_basicModel, Vector3.up * 0.1f, 0.1f);

        private void Reward() => _addMoneyChannel.Invoke(_config.Data.Reward);
    }
}