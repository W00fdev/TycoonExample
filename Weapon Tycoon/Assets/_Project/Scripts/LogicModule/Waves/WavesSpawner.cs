using System;
using System.Collections;
using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.LogicModule.Waves
{
    public class WavesSpawner : MonoBehaviour
    {
        [SerializeField] private Health _wallHealth;
        [SerializeField] private Health _flagHealth;
        [SerializeField] private Transform _wallPivot;
        [SerializeField] private Transform _flagPivot;
        [SerializeField] private Transform _spawnPosition;

        private EnemyFactory _enemyFactory;

        // waveconfig : enemyconfig[] wavedata
        private readonly WaitForSeconds _waiter = new WaitForSeconds(1f);

        public void Initialize(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }
        
        public void StartSpawn()
        {
            StartCoroutine(WavesTimer());
        }

        IEnumerator WavesTimer()
        {
            while (true)
            {
                yield return _waiter;
                Enemy enemy = _enemyFactory.Next(_spawnPosition.position);
                enemy.StartStateMachine();
            }
        }
    }
}