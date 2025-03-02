using System;
using System.Threading;
using _Project.Scripts.Components.Character;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Components.Enemies.States
{
    public class DyingState : IState
    {
        private readonly Animator _animator;
        private readonly Action _diedEvent;

        private CancellationTokenSource _cts;
        private CancellationTokenSource _linkedCts;
        
        private const float SecondsToDie = 2.6f;
        private static readonly int DeathTriggerHash = Animator.StringToHash("Death");
        
        public DyingState(Animator animator, Action diedEvent)
        {
            _animator = animator;
            _diedEvent = diedEvent;
        }
        
        public void Enter()
        {
            _animator.SetTrigger(DeathTriggerHash);
            
            _cts = new CancellationTokenSource();

            _linkedCts =
                CancellationTokenSource.CreateLinkedTokenSource(_cts.Token,
                    _animator.gameObject.GetCancellationTokenOnDestroy());
            
            DelayedDeath().Forget();
        }

        public void Update()
        {
        }

        public void Exit()
        {
            _linkedCts.Cancel();
        }
        
        async UniTaskVoid DelayedDeath()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(SecondsToDie), cancellationToken: _linkedCts.Token);

            _diedEvent.Invoke();
        }
    }
}