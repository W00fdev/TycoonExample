using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Factories.Accessors;
using _Project.Scripts.LogicModule;
using _Project.Scripts.LogicModule.Spawners;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstrappers
{
    public class BattleBootstrap : MonoBehaviour
    {
        [SerializeField] private GameShop _gameShop;

        [Header("Debug only")]
        [ShowInInspector, ReadOnly] private Dictionary<Type, BlasterFactory> _weaponFactories;

        [Inject] private StorageService _storageService;
        [Inject] private PistolFactoryAccessor<PistolFactory> _pistolFactoryAccessor;
        [Inject] private ShotgunFactoryAccessor<ShotgunFactory> _shotgunFactoryAccessor;
        [Inject] private RifleFactoryAccessor<RifleFactory> _rifleFactoryAccessor;
        [Inject] private ProjectileFactoryAccessor<ProjectileFactory> _projectileFactoryAccessor;
        [Inject] private ProjectileFactoryAccessor<ExplosionFactory> _explosionFactoryAccessor;
        [Inject] private BoxFactoryAccessor<BoxFactory> _boxFactoryAccessor;
        [Inject] private BoxFactoryAccessor<LongBoxFactory> _longBoxFactoryAccessor;
        [Inject] private MoneyTextFactoryAccessor _moneyTextFactoryAccessor;        
        
        public async UniTaskVoid Initialize()
        {
            var pistolFactoryTask = PistolFactory.CreateAsync(_storageService);
            var shotgunFactoryTask = ShotgunFactory.CreateAsync(_storageService);
            var rifleFactoryTask = RifleFactory.CreateAsync(_storageService);

            var projectileFactoryTask = ProjectileFactory.CreateAsync(_storageService);
            var explosionFactoryTask = ExplosionFactory.CreateAsync(_storageService);
            
            var boxFactoryTask = BoxFactory.CreateAsync(_storageService);
            var longBoxFactoryTask = LongBoxFactory.CreateAsync(_storageService);
            var moneyTextFactoryTask = MoneyTextFactory.CreateAsync(_storageService);

            var (pistolFactory, shotgunFactory, rifleFactory, projectileFactory, 
                    explosionFactory, boxFactory, longBoxFactory, moneyTextFactory) 
                =
                await UniTask.WhenAll(pistolFactoryTask, shotgunFactoryTask, rifleFactoryTask, projectileFactoryTask,
                    explosionFactoryTask, boxFactoryTask, longBoxFactoryTask, moneyTextFactoryTask);

            _pistolFactoryAccessor.PistolFactory = pistolFactory as PistolFactory;
            _shotgunFactoryAccessor.ShotgunFactory = shotgunFactory as ShotgunFactory;
            _rifleFactoryAccessor.RifleFactory = rifleFactory as RifleFactory;
            _projectileFactoryAccessor.Factory = projectileFactory;
            _explosionFactoryAccessor.Factory = explosionFactory;
            _boxFactoryAccessor.BoxFactory = boxFactory;
            _longBoxFactoryAccessor.BoxFactory = longBoxFactory as LongBoxFactory;
            _moneyTextFactoryAccessor.MoneyTextFactory = moneyTextFactory;
            
            _gameShop.Initialize();
        }
    }
}