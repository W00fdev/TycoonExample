using _Project.Scripts.Animations;
using _Project.Scripts.Components;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Presenters;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.UI.Models;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.LogicModule
{
    public sealed class UpgradeBuyer : MonoBehaviour
    {
        [SerializeField] private SpawnerData[] _spawnerDatas;
        [SerializeField] private SpawnerUpgrader[] _spawnerUpgrades;
        [SerializeField] private SpawnerBuyerInfoView[] _spawnerButtons;
        [SerializeField] private SpawnerUpgraderInfoView[] _upgradeButtons;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private UpgradeController _upgradeController;

        private void Start()
        {
            EnableSpawnerButton(0);
        }

        private void OnEnable()
        {
            EventBus.BuyNextSpawnerPressed += TryBuyNext;
            EventBus.BuySpawnerUpgradePressed += BuyUpgrade;
        }

        private void OnDisable()
        {
            EventBus.BuyNextSpawnerPressed -= TryBuyNext;
            EventBus.BuySpawnerUpgradePressed -= BuyUpgrade;
        }

        public void TryBuyNext()
        {
            int spawnerIndex = _upgradeController.SpawnerLevel;
            
            if (_spawnerDatas.Length <= spawnerIndex)
                return;
            
            var spawnerData = _spawnerDatas[spawnerIndex];
            if (_currencyPipe.SpentCash(spawnerData.BuyPrice) == false)
                return;
            
            EnableUpgraderButton(spawnerIndex);
            _upgradeController.Next(spawnerData);
            
            int nextSpawnerLevel = _upgradeController.SpawnerLevel;
            if (_spawnerButtons.Length > nextSpawnerLevel && _spawnerDatas.Length > nextSpawnerLevel)
                EnableSpawnerButton(nextSpawnerLevel);
        }

        private void EnableUpgraderButton(int spawnerIndex)
        {
            _spawnerButtons[spawnerIndex].DisableSelf();
            _upgradeButtons[spawnerIndex].EnableSelf();
            
            var upgrade = _spawnerUpgrades[spawnerIndex].Upgrade;
            _upgradeButtons[spawnerIndex].SetPriceInfo(upgrade.BuyPrice.ToHeaderMoneyFormat());
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

        private void BuyUpgrade(int spawnerIndex)
        {
            var upgradeData = _spawnerUpgrades[spawnerIndex].Upgrade;
            if (_currencyPipe.SpentCash(upgradeData.BuyPrice) == false)
                return;
         
            var speed = upgradeData.Speed;
            var price = upgradeData.ProductPrice;
            var nextUpgrade = _spawnerUpgrades[spawnerIndex].NextUpgrade();
            
            if (nextUpgrade == null)
            {
                if (_upgradeButtons[spawnerIndex].TryGetComponent(out IButtonUpgrader upgrader))
                    upgrader.DisableButton();
            }
            else
            {
                _upgradeButtons[spawnerIndex].SetPriceInfo(nextUpgrade.BuyPrice.ToHeaderMoneyFormat());
            }

            _spawnerDatas[spawnerIndex].UpdateSpeedAndPrice(speed, price);
        }
    }
}