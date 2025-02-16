using System;
using System.Collections.Generic;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class BattleInstaller : MonoBehaviour
    {
        [Header("Utilities factory prefabs")]
        [SerializeField] private MoneyTextView _moneyTextPrefab;

        [Header("Controllers")]
        [SerializeField] private UpgradeController _upgradeController;

        [Header("Debug only")]
        [ShowInInspector, ReadOnly] private Dictionary<Type, BlasterFactory> _weaponFactories;
        
        public void Initialize(StorageService storageService)
        {
            var glockFactory = new PistolFactory(storageService);
            var shotgunFactory = new ShotgunFactory(storageService);
            var rifleFactory = new RifleFactory(storageService);
            
            var boxFactory = new BoxFactory(storageService);
            var longBoxFactory = new LongBoxFactory(storageService);
            var moneyTextFactory = new MoneyTextFactory(_moneyTextPrefab);

            _weaponFactories = new()
            {
                { typeof(PistolFactory), glockFactory },
                { typeof(ShotgunFactory), shotgunFactory },
                { typeof(RifleFactory), rifleFactory },
            };


            _upgradeController.Initialize(_weaponFactories, boxFactory, moneyTextFactory, longBoxFactory);
        }
    }
}