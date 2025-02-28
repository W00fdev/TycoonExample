using System;
using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Factories.Accessors;
using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
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
        
        private EnemyFactory _enemyFactory;
        private float _delay = 3f;
        
        [Inject] private EnemyFactoryAccessor<EnemyFactory> _enemyFactoryAccessor;

        public void Initialize()
        {
            _enemyFactory = _enemyFactoryAccessor.Factory;
            
            WavesTimer().Forget();
        }

        async UniTaskVoid WavesTimer()
        {
            while (this.GetCancellationTokenOnDestroy().IsCancellationRequested == false)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: this.GetCancellationTokenOnDestroy());
                var enemy = _enemyFactory.Next() as Enemy;

                enemy.transform.position = _spawnPosition.position;
                enemy.Initialize(_wallTarget.IsAlive 
                    ? _wallTarget.transform 
                    : _flagTarget.transform);

            }
        }
    }
}