using System;
using _Project.Scripts.Components;
using _Project.Scripts.Components.Buttons;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.Spawners;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule
{
    public class SpawnerUpgrader : MonoBehaviour
    {
        [SerializeField] private SpawnerData _spawnerData;
        [SerializeField] private SpawnerBuyerInfoView _spawnerButton;
        [SerializeField] private SpawnerUpgraderInfoView _upgradeButton;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private BlasterSpawner _spawner;
        [SerializeField] private int _spawnerIndex;
        
        [Inject] private PersistentProgress _progress;

        private Action _spawnerBought;
        
        public void Initialize(Action spawnerBought, int upgradeIndex)
        {
            _spawnerBought = spawnerBought;
            _spawnerData.Initialize(upgradeIndex);
            
            if (upgradeIndex > 0)
                LoadSpawner();
        }

        private void LoadSpawner()
        {
            UpdateButtonViewAfterUpgrade();
            OpenSpawner();
        }

        public void BuySpawner()
        {
            if (_currencyPipe.TrySpendCash(_spawnerData.BuyPrice) == false)
                return;
            
            OpenSpawner();
            _spawnerBought?.Invoke();
        }
        
        public void BuyUpgrade()
        {
            if (_currencyPipe.TrySpendCash(_spawnerData.UpgradePrice) == false)
                return;

            _spawnerData.Upgrade();
            SaveUpgradeToData();

            UpdateButtonViewAfterUpgrade();
        }
        
        public void ShowBuyButton()
        {
            _spawnerButton.gameObject.SetActive(true);
            _spawnerButton.Initialize(
                Utils.Utils.GetSpawnerKeyName(_spawnerIndex),
                spawnerPrice: _spawnerData.BuyPrice.ToHeaderMoneyFormat(),
                speed: _spawnerData.SpawnerSpeed.ToSpeedFormat(),
                productPrice: _spawnerData.ProductPrice.ToString());
        }

        private void SaveUpgradeToData() => _progress.Data.UpdateSpawnerUpgrade(_spawnerIndex, _spawnerData.Index);

        private void UpdateButtonViewAfterUpgrade()
        {
            if (_spawnerData.IsUpgradeExist() == false)
                _upgradeButton.GetComponent<ButtonSender>().DisableButton();
            else
                _upgradeButton.SetPriceInfo(_spawnerData.UpgradePrice.ToHeaderMoneyFormat());
        }

        private void EnableUpgraderButton()
        {
            _spawnerButton.DisableSelf();
            _upgradeButton.EnableSelf();
            _upgradeButton.SetPriceInfo(_spawnerData.UpgradePrice.ToHeaderMoneyFormat());
        }

        private void OpenSpawner()
        {
            EnableUpgraderButton();
            
            _spawner.gameObject.SetActive(true);
            _spawner.Initialize(_spawnerData);
            _spawner.Resolve();
        }
    }
}