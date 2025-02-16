using System;
using System.Collections.Generic;
using System.Text;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Presenters;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using _Project.Scripts.UI.Presenters;
using Playgama;
using Playgama.Modules.Advertisement;
using Playgama.Modules.Platform;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class SystemInstaller : MonoBehaviour
    {
        [Header("Utilities factory prefabs")]
        [SerializeField] private AssetReferenceGameObject _moneyTextPrefab;
        
        [Header("Controllers")]
        [SerializeField] private UpgradeController _upgradeController;
        [SerializeField] private CurrencyPipe _pipe;

        [Header("Debug only")]
        [ShowInInspector, ReadOnly] private Dictionary<Type, BlasterFactory> _weaponFactories;

        [Space] [ShowInInspector, ReadOnly] private BankStorage _bankStorage;
        [Space] [ShowInInspector, ReadOnly] private WalletStorage _walletStorage;

        private void Awake()
        {
            var handle = _moneyTextPrefab.LoadAssetAsync();
            handle.WaitForCompletion();
            
            var glockFactory = new PistolFactory();
            var shotgunFactory = new ShotgunFactory();
            var rifleFactory = new RifleFactory();
            
            var boxFactory = new BoxFactory();
            var longBoxFactory = new LongBoxFactory();
            var moneyTextFactory = new MoneyTextFactory(handle.Result.GetComponent<MoneyTextView>());

            _weaponFactories = new()
            {
                { typeof(PistolFactory), glockFactory },
                { typeof(ShotgunFactory), shotgunFactory },
                { typeof(RifleFactory), rifleFactory },
            };

            _bankStorage = new BankStorage(0);
            _walletStorage = new WalletStorage(0);
            
            _pipe.Initialize(_bankStorage, _walletStorage);
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