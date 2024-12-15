﻿using System;
using System.Collections.Generic;
using System.Text;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Presenters;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using Playgama;
using Playgama.Modules.Advertisement;
using Playgama.Modules.Platform;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class SystemInstaller : MonoBehaviour
    {
        [Header("Weapon factory prefabs")]
        [SerializeField] private WeaponView _pistolPrefab;
        [SerializeField] private WeaponView _shotgunPrefab;
        [SerializeField] private WeaponView _riflePrefab;

        [Header("Utilities factory prefabs")]
        [SerializeField] private PooledView _boxPrefab;
        [SerializeField] private PooledView _longBoxPrefab;
        [SerializeField] private PooledView _moneyTextPrefab;
        
        [Header("Controllers")]
        [SerializeField] private UpgradeController _upgradeController;
        [SerializeField] private CurrencyPipe _pipe;

        [Header("Debug only")]
        [ShowInInspector] private Dictionary<Type, BlasterFactory> _weaponFactories;

        [Space] [ShowInInspector] private BankStorage _bankStorage;
        [Space] [ShowInInspector] private WalletStorage _walletStorage;

        private void Awake()
        {
            var glockFactory = new PistolFactory(_pistolPrefab);
            var shotgunFactory = new ShotgunFactory(_shotgunPrefab);
            var rifleFactory = new RifleFactory(_riflePrefab);
            
            var boxFactory = new BoxFactory(_boxPrefab);
            var longBoxFactory = new BoxFactory(_longBoxPrefab);
            var moneyTextFactory = new MoneyTextFactory(_moneyTextPrefab);
            
            _weaponFactories = new()
            {
                { typeof(PistolFactory), glockFactory },
                { typeof(ShotgunFactory), shotgunFactory },
                { typeof(RifleFactory), rifleFactory },
            };

            _bankStorage = new BankStorage(0);
            _walletStorage = new WalletStorage(0);
            
            _pipe.Initialize(_bankStorage, _walletStorage);
            glockFactory.EntityReturned += _pipe.ProductConsumed;
            shotgunFactory.EntityReturned += _pipe.ProductConsumed;
            rifleFactory.EntityReturned += _pipe.ProductConsumed;
            
            _upgradeController.Initialize(_weaponFactories, boxFactory, moneyTextFactory, longBoxFactory);

#if  !UNITY_EDITOR
            Bridge.platform.SendMessage(PlatformMessage.GameReady);

            Debug.Log(Bridge.platform.name);
            
            Bridge.advertisement.ShowInterstitial();
            Bridge.advertisement.interstitialStateChanged += OnInterstitialStateChanged;
#endif
        }

        private void OnDestroy()
        {
#if  !UNITY_EDITOR
            Bridge.advertisement.interstitialStateChanged -= OnInterstitialStateChanged;
#endif
        }

        private void OnInterstitialStateChanged(InterstitialState state)
        {
            switch (state)
            {
                case InterstitialState.Loading:
                    Debug.Log("Loading");
                    break;
                case InterstitialState.Opened:
                    Debug.Log("Opened");
                    break;
                case InterstitialState.Closed:
                    Debug.Log("Closed");
                    break;
                case InterstitialState.Failed:
                    Debug.Log("Failed");
                    break;
            }
        }
        
    }
}