using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.LogicModule;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstrappers
{
    public class BattleBootstrap : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private SpawnerUpgrader _spawnerUpgrader;
        [SerializeField] private UpgradeController _upgradeController;

        [Header("Debug only")]
        [ShowInInspector, ReadOnly] private Dictionary<Type, BlasterFactory> _weaponFactories;

        private StorageService _storageService;
        
        [Inject]
        public void Construct(StorageService service)
        {
            _storageService = service;
        }
        
        public async UniTaskVoid Initialize()
        {
            var pistolFactoryTask = PistolFactory.CreateAsync(_storageService);
            var shotgunFactoryTask = ShotgunFactory.CreateAsync(_storageService);
            var rifleFactoryTask = RifleFactory.CreateAsync(_storageService);
            
            var boxFactoryTask = BoxFactory.CreateAsync(_storageService);
            var longBoxFactoryTask = LongBoxFactory.CreateAsync(_storageService);
            var moneyTextFactoryTask = MoneyTextFactory.CreateAsync(_storageService);

            var (pistolFactory, shotgunFactory, rifleFactory, boxFactory, longBoxFactory, moneyTextFactory) =
                await UniTask.WhenAll(pistolFactoryTask, shotgunFactoryTask, rifleFactoryTask, boxFactoryTask, longBoxFactoryTask, moneyTextFactoryTask);
            
            _weaponFactories = new()
            {
                { typeof(PistolFactory), pistolFactory },
                { typeof(ShotgunFactory), shotgunFactory },
                { typeof(RifleFactory), rifleFactory },
            };

            _upgradeController.Initialize(_weaponFactories, boxFactory, moneyTextFactory, longBoxFactory);
            _spawnerUpgrader.Initialize();
        }
    }
}