using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Factories.Accessors;
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

        [Inject] private StorageService _storageService;
        [Inject] private PistolFactoryAccessor<PistolFactory> _pistolFactoryAccessor;
        [Inject] private ShotgunFactoryAccessor<ShotgunFactory> _shotgunFactoryAccessor;
        [Inject] private RifleFactoryAccessor<RifleFactory> _rifleFactoryAccessor;
        [Inject] private BoxFactoryAccessor<BoxFactory> _boxFactoryAccessor;
        [Inject] private BoxFactoryAccessor<LongBoxFactory> _longBoxFactoryAccessor;
        [Inject] private MoneyTextFactoryAccessor _moneyTextFactoryAccessor;        
        
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

            _pistolFactoryAccessor.PistolFactory = pistolFactory as PistolFactory;
            _shotgunFactoryAccessor.ShotgunFactory = shotgunFactory as ShotgunFactory;
            _rifleFactoryAccessor.RifleFactory = rifleFactory as RifleFactory;
            _boxFactoryAccessor.BoxFactory = boxFactory;
            _longBoxFactoryAccessor.BoxFactory = longBoxFactory as LongBoxFactory;
            _moneyTextFactoryAccessor.MoneyTextFactory = moneyTextFactory;
            
            _upgradeController.Initialize();
            _spawnerUpgrader.Initialize();
        }
    }
}