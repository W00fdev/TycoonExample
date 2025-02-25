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
        
        private BoxFactory _longBoxFactory;
        private BoxFactory _boxFactory;
        private MoneyTextFactory _moneyTextFactory;

        private BlasterFactoryResolver _resolver;
        private int _spawnerLevel;

        [Inject] private PersistentProgress _progress;
        
        
        public void Initialize(Dictionary<Type, BlasterFactory> factories,
            BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, BoxFactory longBoxFactory)
        {
            _longBoxFactory = 
            _boxFactory = boxFactory;
            _longBoxFactory = longBoxFactory;
            _moneyTextFactory = moneyTextFactory;
            
            _resolver = new BlasterFactoryResolver(factories);
        }

        public void Open(SpawnerData spawnerData, int index)
        {
            if (_spawnerLevel >= _spawners.Count)
                return;
            
            var spawner = _spawners[_spawnerLevel++];
            _progress.Data.Spawners = _spawnerLevel;
            
            spawner.gameObject.SetActive(true);
            // Косяк, не засунули в резолвер
            var boxFactory = (spawner is RifleSpawner) ? _longBoxFactory : _boxFactory;
            spawner.Initialize(boxFactory, _moneyTextFactory, spawnerData);
            _resolver.Resolve(spawner);
        }
    }
}