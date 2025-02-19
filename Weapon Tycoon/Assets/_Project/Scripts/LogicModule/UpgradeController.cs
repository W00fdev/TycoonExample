using System;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using _Project.Scripts.UI.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts
{
    public sealed class UpgradeController : MonoBehaviour
    {
        [SerializeField] private List<BlasterSpawner> _spawners;
        
        private BoxFactory _longBoxFactory;
        private BoxFactory _boxFactory;
        private MoneyTextFactory _moneyTextFactory;
        
        private BlasterFactoryResolver _resolver;
        private int _spawnerLevel;

        public int SpawnerLevel => _spawnerLevel;
        
        public void Initialize(Dictionary<Type, BlasterFactory> factories,
            BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, BoxFactory longBoxFactory)
        {
            _longBoxFactory = 
            _boxFactory = boxFactory;
            _longBoxFactory = longBoxFactory;
            _moneyTextFactory = moneyTextFactory;
            
            _resolver = new BlasterFactoryResolver(factories);
        }

        public BlasterSpawner Next(SpawnerData spawnerData)
        {
            if (_spawnerLevel >= _spawners.Count)
                return null;
            
            var spawner = _spawners[_spawnerLevel++];
            PersistentProgress.Instance.Spawners = _spawnerLevel;
            
            spawner.gameObject.SetActive(true);
            var boxFactory = (spawner is RifleSpawner) ? _longBoxFactory : _boxFactory;
            spawner.Initialize(boxFactory, _moneyTextFactory, spawnerData);
            _resolver.Resolve(spawner);
            
            return spawner;
        }
    }
}