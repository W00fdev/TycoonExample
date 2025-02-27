using System.Threading;
using _Project.Scripts.Components.Character;
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
        private CancellationTokenSource _cts;
        private CancellationTokenSource _linkedCts;

        private const string WalkBoolean = "Walk";
        private static readonly int WalkBooleanHash = Animator.StringToHash(WalkBoolean); 

        public WalkingState(IStateMachineEnemy stateMachineEnemy)
        {
            _stateMachineEnemy = stateMachineEnemy;
            
            _agent = _stateMachineEnemy.Agent;
            _animator = _stateMachineEnemy.Animator;

            _target = stateMachineEnemy.Target;
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
        }

        public void Update()
        {
        }

        public void Exit()
        {
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