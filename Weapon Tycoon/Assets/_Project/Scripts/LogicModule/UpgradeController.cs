using System;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
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
        
        private WeaponFactoryResolver _resolver;
        private int _upgradeLevel;

        public int UpgradeLevel => _upgradeLevel;
        
        public void Initialize(Dictionary<Type, BlasterFactory> factories,
            BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, BoxFactory longBoxFactory)
        {
            _longBoxFactory = 
            _boxFactory = boxFactory;
            _longBoxFactory = longBoxFactory;
            _moneyTextFactory = moneyTextFactory;
            
            _resolver = new WeaponFactoryResolver(factories);
        }

        public BlasterSpawner Next(WeaponSpawnerData spawnerData)
        {
            if (_upgradeLevel >= _spawners.Count)
                return null;
            
            var spawner = _spawners[_upgradeLevel++];
            spawner.gameObject.SetActive(true);
            var boxFactory = (spawner is RifleSpawner) ? _longBoxFactory : _boxFactory;
            spawner.Initialize(boxFactory, _moneyTextFactory, spawnerData);
            _resolver.Resolve(spawner);
            
            return spawner;
        }
    }
}