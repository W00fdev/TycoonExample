﻿using _Project.Scripts.Animations;
using _Project.Scripts.Components;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Presenters;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.UI.Models;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.LogicModule
{
    public sealed class UpgradeBuyer : MonoBehaviour
    {
        [SerializeField] private SpawnerData[] _spawnerDatas;
        [SerializeField] private SpawnerUpgrader[] _spawnerUpgrades;
        [SerializeField] private SpawnerBuyerInfoView[] _spawnerButtons;
        [SerializeField] private SpawnerUpgraderInfoView[] _upgradePriceButtons;
        [SerializeField] private SpawnerUpgraderInfoView[] _upgradeSpeedButtons;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private UpgradeController _upgradeController;

        private void Start()
        {
            EnableSpawnerButton(0);
        }

        private void OnEnable()
        {
            EventBus.BuyNextSpawnerPressed += TryBuyNext;
            EventBus.BuySpawnerUpgradePricePressed += BuyPriceUpgrade;
            EventBus.BuySpawnerUpgradeSpeedPressed += BuySpeedUpgrade;
        }

        private void OnDisable()
        {
            EventBus.BuyNextSpawnerPressed -= TryBuyNext;
            EventBus.BuySpawnerUpgradePricePressed -= BuyPriceUpgrade;
            EventBus.BuySpawnerUpgradeSpeedPressed -= BuySpeedUpgrade;
        }

        public void TryBuyNext()
        {
            int spawnerIndex = _upgradeController.SpawnerLevel;
            
            if (_spawnerDatas.Length <= spawnerIndex)
                return;
            
            var spawnerData = _spawnerDatas[spawnerIndex];
            if (_currencyPipe.SpentCash(spawnerData.BuyPrice) == false)
                return;
            
            EnableUpgraderButtons(spawnerIndex);
            _upgradeController.Next(spawnerData);
            
            int nextSpawnerLevel = _upgradeController.SpawnerLevel;
            if (_spawnerButtons.Length > nextSpawnerLevel && _spawnerDatas.Length > nextSpawnerLevel)
                EnableSpawnerButton(nextSpawnerLevel);
        }

        private void EnableUpgraderButtons(int spawnerIndex)
        {
            _spawnerButtons[spawnerIndex].gameObject.SetActive(false);
            
            var upgradePriceButton = _upgradePriceButtons[spawnerIndex];
            var upgradeSpeedButton = _upgradeSpeedButtons[spawnerIndex];
            upgradePriceButton.gameObject.SetActive(true);
            upgradeSpeedButton.gameObject.SetActive(true);

            var data = _spawnerDatas[spawnerIndex];
            upgradePriceButton.UpdateInfo(0.ToHeaderMoneyFormat(), data.ProductPrice.ToString());
            upgradeSpeedButton.UpdateInfo(0.ToHeaderMoneyFormat(), data.SpawnerSpeed.ToSpeedFormat());

            var priceUpgrade = _spawnerUpgrades[spawnerIndex].PriceUpgrade;
            var speedUpgrade = _spawnerUpgrades[spawnerIndex].SpeedUpgrade;
            upgradePriceButton.UpdateInfo(priceUpgrade.BuyPrice.ToHeaderMoneyFormat(), priceUpgrade.ProductPrice.ToString());
            upgradeSpeedButton.UpdateInfo(speedUpgrade.BuyPrice.ToHeaderMoneyFormat(), speedUpgrade.Speed.ToSpeedFormat());
        }

        private void SetDisableSpawnerButton(int upgradeLevel)
        {
            _spawnerButtons[upgradeLevel].GetComponent<ButtonAnimation>().SetOnReleaseCallback(
                () => _spawnerButtons[upgradeLevel].gameObject.SetActive(false));
        }

        private void EnableSpawnerButton(int nextUpgradeLevel)
        {
            var data = _spawnerDatas[nextUpgradeLevel];
            data.Initialize();
            
            _spawnerButtons[nextUpgradeLevel].gameObject.SetActive(true);
            _spawnerButtons[nextUpgradeLevel].Initialize(
                spawnerName:    data.SpawnerName,
                spawnerPrice:   data.BuyPrice.ToHeaderMoneyFormat(),
                speed:          data.SpawnerSpeed.ToSpeedFormat(),
                productPrice:   data.ProductPrice.ToString());
        }
        
        private void BuySpeedUpgrade(int spawnerIndex)
        {
            var speedUpgradeData = _spawnerUpgrades[spawnerIndex].SpeedUpgrade;
            if (_currencyPipe.SpentCash(speedUpgradeData.BuyPrice) == false)
                return;
            
            var speed = speedUpgradeData.Speed;
            var upgradeSpeedButton = _upgradeSpeedButtons[spawnerIndex];
            var nextSpeedUpgrade = _spawnerUpgrades[spawnerIndex].NextSpeedUpgrade();

            if (nextSpeedUpgrade == null)
            {
                //upgradeSpeedButton.UpdateInfo("max", "max");
                if (upgradeSpeedButton.TryGetComponent(out IButtonUpgrader upgrader))
                    upgrader.DisableButton();
            }
            else
            {
                upgradeSpeedButton.UpdateInfo(nextSpeedUpgrade.BuyPrice.ToHeaderMoneyFormat(), nextSpeedUpgrade.Speed.ToSpeedFormat());
            }

            _spawnerDatas[spawnerIndex].UpdateSpeed(speed);
        }

        private void BuyPriceUpgrade(int spawnerIndex)
        {
            var priceUpgrade = _spawnerUpgrades[spawnerIndex].PriceUpgrade;
            if (_currencyPipe.SpentCash(priceUpgrade.BuyPrice) == false)
                return;
            
            var price = priceUpgrade.ProductPrice;
            var upgradePriceButton = _upgradePriceButtons[spawnerIndex];
            var nextPriceUpgrade = _spawnerUpgrades[spawnerIndex].NextPriceUpgrade();

            if (nextPriceUpgrade == null)
            {
                //upgradePriceButton.UpdateInfo("max", "max");
                if (upgradePriceButton.TryGetComponent(out IButtonUpgrader upgrader))
                    upgrader.DisableButton();
            }
            else
            {
                upgradePriceButton.UpdateInfo(nextPriceUpgrade.BuyPrice.ToHeaderMoneyFormat(), nextPriceUpgrade.ProductPrice.ToString());
            }
            
            _spawnerDatas[spawnerIndex].UpdatePrice(price);
        }
    }
}