using System.Threading;
using _Project.Scripts.Components.Character;
using _Project.Scripts.Infrastructure.Data.Enemies;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Components.Enemies.States
{
    public class WalkingState : IState
    {
        private readonly IStateMachineEnemy _stateMachineEnemy;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;

        private Transform _target;
        private EnemyConfig _enemyConfig;
        private CancellationTokenSource _cts;
        private CancellationTokenSource _linkedCts;

        private static readonly int WalkBooleanHash = Animator.StringToHash("Walk");
        private static readonly int SpeedMagnitude = Animator.StringToHash("SpeedMagnitude");

        public WalkingState(IStateMachineEnemy stateMachineEnemy, EnemyConfig enemyConfig)
        {
            _stateMachineEnemy = stateMachineEnemy;
            
            _agent = _stateMachineEnemy.Agent;
            _animator = _stateMachineEnemy.Animator;
            _target = stateMachineEnemy.Target;

            UpdateConfig(enemyConfig);
        }

        public void UpdateConfig(EnemyConfig enemyConfig)
        {
            _enemyConfig = enemyConfig;
            _agent.speed = _enemyConfig.Data.Speed;
        }
        
        public void Enter()
        {
            _target = _stateMachineEnemy.Target;
            _agent.SetDestination(_target.position);
            _animator.SetBool(WalkBooleanHash, true);
            
            _cts = new CancellationTokenSource();

            _linkedCts =
                CancellationTokenSource.CreateLinkedTokenSource(_cts.Token,
                    _agent.gameObject.GetCancellationTokenOnDestroy());
            
            OptimizedChecker().Forget();
            
            _animator.SetFloat(SpeedMagnitude, _enemyConfig.Data.Speed);
        }

        public void Update()
        {
        }

        public void Exit()
        {
            _animator.SetFloat(SpeedMagnitude, 0f);
            _animator.SetBool(WalkBooleanHash, false);
            _linkedCts.Cancel();
            _target = null;
        }

        async UniTaskVoid OptimizedChecker()
        {
            while (_linkedCts.IsCancellationRequested == false)
            {
                await UniTask.DelayFrame(3, cancellationToken: _linkedCts.Token);
                
                if (!_agent.isStopped &&
                    !(Vector3.Distance(_agent.transform.position, _target.position) <=
                      _agent.stoppingDistance)) continue;
                
                _agent.isStopped = true;
                _stateMachineEnemy.SwitchState<MeleeAttackState>();
            }
        }
    }
}