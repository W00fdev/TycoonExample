using System;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.Spawners;
using _Project.Scripts.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule
{
    public sealed class UpgradeController : MonoBehaviour
    {
        [SerializeField] private List<BlasterSpawner> _spawners;
        private int _spawnerLevel;

        [Inject] private PersistentProgress _progress;
        
        public void Initialize()
        {

        }

        public void Open(SpawnerData spawnerData, int index)
        {
            if (_spawnerLevel >= _spawners.Count)
                return;
            
            var spawner = _spawners[_spawnerLevel++];
            _progress.Data.Spawners = _spawnerLevel;
            
            spawner.gameObject.SetActive(true);
            spawner.Initialize(spawnerData);
            spawner.Resolve();
        }
    }
}