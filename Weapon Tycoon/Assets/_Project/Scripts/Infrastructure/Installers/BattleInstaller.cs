using System;
using System.Collections.Generic;
using _Project.Scripts.LogicModule;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class BattleInstaller : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private UpgradeShop _upgradeShop;
        [SerializeField] private UpgradeController _upgradeController;

        [Header("Debug only")]
        [ShowInInspector, ReadOnly] private Dictionary<Type, BlasterFactory> _weaponFactories;
        
        public async UniTaskVoid Initialize(StorageService storageService)
        {
            var pistolFactoryTask = PistolFactory.CreateAsync(storageService);
            var shotgunFactoryTask = ShotgunFactory.CreateAsync(storageService);
            var rifleFactoryTask = RifleFactory.CreateAsync(storageService);
            
            var boxFactoryTask = BoxFactory.CreateAsync(storageService);
            var longBoxFactoryTask = LongBoxFactory.CreateAsync(storageService);
            var moneyTextFactoryTask = MoneyTextFactory.CreateAsync(storageService);

            var (pistolFactory, shotgunFactory, rifleFactory, boxFactory, longBoxFactory, moneyTextFactory) =
                await UniTask.WhenAll(pistolFactoryTask, shotgunFactoryTask, rifleFactoryTask, boxFactoryTask, longBoxFactoryTask, moneyTextFactoryTask);
            
            _weaponFactories = new()
            {
                { typeof(PistolFactory), pistolFactory },
                { typeof(ShotgunFactory), shotgunFactory },
                { typeof(RifleFactory), rifleFactory },
            };

            _upgradeController.Initialize(_weaponFactories, boxFactory, moneyTextFactory, longBoxFactory);
            _upgradeShop.Initialize();
        }
    }
}