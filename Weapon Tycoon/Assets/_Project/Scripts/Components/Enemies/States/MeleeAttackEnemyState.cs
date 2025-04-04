using System;
using System.Threading;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Infrastructure.Data.Enemies;
using _Project.Scripts.Infrastructure.States;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Components.Enemies.States
{
    public class MeleeAttackEnemyState : ITickableState
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
        
        // No animation events (reflection is too slow)
        private const float AttackAnimationTiming = 0.9f;

        private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

        public MeleeAttackEnemyState(IStateMachineEnemy stateMachineEnemy, LayerMask targetMask, EnemyConfig enemyConfig)
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
                
            _stateMachineEnemy.SwitchState<WalkingEnemyState>();
        }

        private void StartAttacking()
        {
            _cts = new CancellationTokenSource();

            _linkedCts =
                CancellationTokenSource.CreateLinkedTokenSource(_cts.Token,
                    _animator.GetCancellationTokenOnDestroy());
            
            AttackTimer().Forget();
        }

        public void Tick()
        {
        }

        public void Exit()
        {
            _linkedCts?.Cancel();
        }
        
        async UniTaskVoid AttackTimer()
        {
            while (_linkedCts.IsCancellationRequested == false 
                   && _targetHealth && _targetHealth.IsAlive)
            {
                _animator.SetTrigger(AttackTriggerHash);
                
                await UniTask.Delay(TimeSpan.FromSeconds(AttackAnimationTiming), cancellationToken: _linkedCts.Token);
                _targetHealth.TakeDamage(_damage);

                await UniTask.Delay(TimeSpan.FromSeconds(_atkCooldown - AttackAnimationTiming), cancellationToken: _linkedCts.Token);
            }
            
            if (!_targetHealth)
                _stateMachineEnemy.SwitchState<WalkingEnemyState>();
            else if (_targetHealth.IsAlive == false)
                _stateMachineEnemy.SwitchState<WalkingEnemyState>();
        }
    }
}