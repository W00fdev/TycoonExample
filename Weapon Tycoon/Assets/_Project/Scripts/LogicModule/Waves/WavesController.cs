using System;
using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule.Waves
{
    public class WavesController : MonoBehaviour
    {
        [SerializeField] private Health _wallTarget;
        [SerializeField] private Health _flagTarget;
        [SerializeField] private Transform _spawnPosition;
        
        // waveconfig : enemyconfig[] wavedata
        private float _delay = 1f;
        
        [Inject] private ObjectPoolService _poolService;

        public void Initialize()
        {
            WavesTimer().Forget();
        }

        async UniTaskVoid WavesTimer()
        {
            while (this.GetCancellationTokenOnDestroy().IsCancellationRequested == false)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: this.GetCancellationTokenOnDestroy());
                var enemy = _poolService.Next(ObjectPoolService.DefenceType.Enemy1) as Enemy;

                enemy!.transform.position = _spawnPosition.position;
                enemy.Initialize(_wallTarget.IsAlive 
                    ? _wallTarget.transform 
                    : _flagTarget.transform);

            }
        }
    }
}