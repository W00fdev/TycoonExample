using System;
using System.Threading;
using _Project.Scripts.Components.Character;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Components.Enemies.States
{
    public class MeleeAttackState : IState
    {
        private readonly IStateMachineEnemy _stateMachineEnemy;
        private readonly LayerMask _targetMask;
        private readonly Animator _animator;
        private readonly Collider[] _targetColliders;

        private int _damage;
        private Health _target;
        private CancellationTokenSource _cts;
        private CancellationTokenSource _linkedCts;

        private const float AttackCooldown = 2.5f;
        private const string AttackTrigger = "Attack";
        private static readonly int AttackTriggerHash = Animator.StringToHash(AttackTrigger);

        public MeleeAttackState(IStateMachineEnemy stateMachineEnemy, LayerMask targetMask, int damage)
        {
            _stateMachineEnemy = stateMachineEnemy;
            _targetMask = targetMask;
            _damage = damage;
            
            _targetColliders = new Collider[1];
        }

        public void UpdateDamage(int damage)
            => _damage = damage;
        
        public void Enter()
        {
            if (Physics.OverlapSphereNonAlloc
                    (_animator.transform.position, 1f, _targetColliders, _targetMask.value) > 0)
            {
                if (_targetColliders[0].TryGetComponent(out _target))
                    StartAttacking();
                        
                //_stateMachineEnemy.SwitchState<WalkingState>();
                return;
            }
                
            _stateMachineEnemy.SwitchState<WalkingState>();
        }

        private void StartAttacking()
        {
            _cts = new CancellationTokenSource();

            _linkedCts =
                CancellationTokenSource.CreateLinkedTokenSource(_cts.Token,
                    _animator.gameObject.GetCancellationTokenOnDestroy());
            
            AttackTimer().Forget();
        }

        public void Update()
        {
        }

        public void Exit()
        {
            _linkedCts.Cancel();
        }
        
        async UniTaskVoid AttackTimer()
        {
            while (_linkedCts.IsCancellationRequested == false 
                   && _target && _target.IsAlive)
            {
                _animator.SetTrigger(AttackTriggerHash);
                _target.TakeDamage(_damage);
                
                await UniTask.Delay(TimeSpan.FromSeconds(AttackCooldown), cancellationToken: _linkedCts.Token);
            }
            
            if (!_target)
                _stateMachineEnemy.SwitchState<WalkingState>();
            
            if (_target.IsAlive == false)
                _stateMachineEnemy.SwitchState<WalkingState>();
        }
    }
}