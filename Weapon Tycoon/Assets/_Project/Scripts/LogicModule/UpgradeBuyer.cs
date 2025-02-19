using _Project.Scripts.Animations;
using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Models;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.LogicModule
{
    public class UpgradeBuyer : MonoBehaviour
    {
        [SerializeField] private SpawnerData[] _spawnerDatas;
        [SerializeField] private SpawnerBuyerInfoView[] _spawnerButtons;
        [SerializeField] private SpawnerUpgraderInfoView[] _upgradeButtons;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private UpgradeController _upgradeController;
        
        public void Initialize()
        {
            int spawners = PersistentProgress.Instance.Spawners;
            if (spawners == 0)
                EnableSpawnerButton(0);
            else
                LoadSpawners(spawners);
        }

        public void LoadSpawners(int spawnersCount)
        {
            var upgrades = PersistentProgress.Instance.SpawnerUpgrade;
            int index;
            for (index = 0; index < spawnersCount; index++)
                LoadSpawner(index < upgrades.Count ? upgrades[index] : null, index);
            
            if (_spawnerButtons.Length > index && _spawnerDatas.Length > index)
                EnableSpawnerButton(index);
        }

        private void LoadSpawner(SpawnerUpgradeDTO data, int spawnerIndex)
        {
            if (_spawnerDatas.Length <= spawnerIndex)
                return;
            
            var spawnerData = _spawnerDatas[spawnerIndex];
            spawnerData.Initialize();
            
            if (data != null)
                spawnerData.SetData(data);
            
            if (spawnerData.UpgradeIndex >= spawnerData.TotalUpgradesCount)
            {
                if (_upgradeButtons[spawnerIndex].TryGetComponent(out IButtonUpgrader upgrader))
                    upgrader.DisableButton();
            }
            else
            {
                _upgradeButtons[spawnerIndex].SetPriceInfo(spawnerData.NextUpgradePrice.ToHeaderMoneyFormat());
            }
            
            EnableUpgraderButton(spawnerIndex);
            _upgradeController.Next(spawnerData);
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
            
            var upgradePrice = _spawnerDatas[spawnerIndex].NextUpgradePrice;
            _upgradeButtons[spawnerIndex].SetPriceInfo(upgradePrice.ToHeaderMoneyFormat());
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
                Utils.Utils.GetSpawnerName(nextUpgradeLevel),
                spawnerPrice:   data.BuyPrice.ToHeaderMoneyFormat(),
                speed:          data.SpawnerSpeed.ToSpeedFormat(),
                productPrice:   data.ProductPrice.ToString());
        }

        private void BuyUpgrade(int spawnerIndex)
        {
            var upgradeData = _spawnerDatas[spawnerIndex];
            if (_currencyPipe.SpentCash(upgradeData.NextUpgradePrice) == false)
                return;
         
            var nextUpgrade = _spawnerDatas[spawnerIndex].ApplyNextUpgrade();
            PersistentProgress.Instance.UpdateSpawnerUpgrade(_spawnerDatas[spawnerIndex].DTO, spawnerIndex);
            
            if (nextUpgrade == null)
            {
                if (_upgradeButtons[spawnerIndex].TryGetComponent(out IButtonUpgrader upgrader))
                    upgrader.DisableButton();
            }
            else
            {
                _upgradeButtons[spawnerIndex].SetPriceInfo(upgradeData.NextUpgradePrice.ToHeaderMoneyFormat());
            }
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
    }
}