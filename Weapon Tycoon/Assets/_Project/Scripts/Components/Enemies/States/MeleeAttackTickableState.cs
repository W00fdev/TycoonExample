using System;
using System.Threading;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Infrastructure.Data.Enemies;
using _Project.Scripts.Infrastructure.States;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Components.Enemies.States
{
    public class MeleeAttackTickableState : ITickableState
    {
        private readonly IStateMachineEnemy _stateMachineEnemy;
        private readonly LayerMask _targetMask;
        private readonly Animator _animator;
        private readonly RaycastHit[] _hits;

        private int _damage;
        private float _atkCooldown;
        private RestorableHealth _targetHealth;
        private EnemyConfig _enemyConfig;
        private CancellationTokenSource _cts;
        private CancellationTokenSource _linkedCts;

        private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

        public MeleeAttackTickableState(IStateMachineEnemy stateMachineEnemy, LayerMask targetMask, EnemyConfig enemyConfig)
        {
            _stateMachineEnemy = stateMachineEnemy;
            _targetMask = targetMask;
            _animator = _stateMachineEnemy.Animator;
            _hits = new RaycastHit[1];

            UpdateConfig(enemyConfig);
        }

        private void UpdateConfig(EnemyConfig enemyConfig)
        {
            _enemyConfig = enemyConfig;
            
            _damage = _enemyConfig.Data.Damage;
            _atkCooldown = _enemyConfig.Data.AtkCooldown;
        }

        public void Enter()
        {
            Debug.Log("Entering AttackState");
            
            if (Physics.BoxCastNonAlloc(
                    _animator.transform.position, Vector3.one, _animator.transform.forward,
                    _hits, Quaternion.identity, 1f, _targetMask.value) > 0)
            {
                Debug.Log("Found overlap: " + _hits[0]);
                if (_hits[0].collider.TryGetComponent(out _targetHealth))
                    StartAttacking();
                
                return;
            }
                
            _stateMachineEnemy.SwitchState<WalkingTickableState>();
        }

        private void StartAttacking()
        {
            Debug.Log("Start Attack");
            
            _cts = new CancellationTokenSource();

            _linkedCts =
                CancellationTokenSource.CreateLinkedTokenSource(_cts.Token,
                    _animator.gameObject.GetCancellationTokenOnDestroy());
            
            AttackTimer().Forget();
        }

        public void Tick()
        {
        }

        public void Exit()
        {
            _linkedCts.Cancel();
        }
        
        async UniTaskVoid AttackTimer()
        {
            while (_linkedCts.IsCancellationRequested == false 
                   && _targetHealth && _targetHealth.IsAlive)
            {
                _animator.SetTrigger(AttackTriggerHash);
                _targetHealth.TakeDamage(_damage);
                
                await UniTask.Delay(TimeSpan.FromSeconds(_atkCooldown), cancellationToken: _linkedCts.Token);
            }
            
            if (!_targetHealth)
                _stateMachineEnemy.SwitchState<WalkingTickableState>();
            else if (_targetHealth.IsAlive == false)
                _stateMachineEnemy.SwitchState<WalkingTickableState>();
        }
    }
}