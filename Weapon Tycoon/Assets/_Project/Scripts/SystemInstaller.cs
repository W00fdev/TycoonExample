using System;
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

namespace _Project.Scripts
{
    public class SystemInstaller : MonoBehaviour
    {
        [Header("Weapon factory prefabs")]
        [SerializeField] private WeaponView _glockPrefab;
        [SerializeField] private WeaponView _akPrefab;

        [Header("Utilities factory prefabs")]
        [SerializeField] private PooledView _boxPrefab;
        [SerializeField] private PooledView _moneyTextPrefab;
        
        [Header("Controllers")]
        [SerializeField] private UpgradeController _upgradeController;
        [SerializeField] private CurrencyPipe _pipe;

        [Header("Debug only")]
        [ShowInInspector] private Dictionary<Type, WeaponFactory> _weaponFactories;

        [Space] [ShowInInspector] private BankStorage _bankStorage;
        [Space] [ShowInInspector] private WalletStorage _walletStorage;

        private void Awake()
        {

            string a = "a";
            string ba = "b" + a;

            a = "b";
            
            var glockFactory = new GlockFactory(_glockPrefab);
            var akFactory = new AkFactory(_akPrefab);
            var boxFactory = new BoxFactory(_boxPrefab);
            var moneyTextFactory = new MoneyTextFactory(_moneyTextPrefab);
            
            _weaponFactories = new()
            {
                { typeof(GlockFactory), glockFactory },
                { typeof(AkFactory), akFactory },
            };

            _bankStorage = new BankStorage(0);
            _walletStorage = new WalletStorage(0);
            
            _pipe.Initialize(_bankStorage, _walletStorage);
            glockFactory.EntityReturned += _pipe.ProductConsumed;
            akFactory.EntityReturned += _pipe.ProductConsumed;
            
            _upgradeController.Initialize(_weaponFactories, boxFactory, moneyTextFactory);

            Bridge.platform.SendMessage(PlatformMessage.GameReady);
            
            Debug.Log(Bridge.platform.name);
            
            Bridge.advertisement.ShowInterstitial();
            Bridge.advertisement.interstitialStateChanged += OnInterstitialStateChanged;
        }

        private void OnDestroy()
        {
            Bridge.advertisement.interstitialStateChanged -= OnInterstitialStateChanged;
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