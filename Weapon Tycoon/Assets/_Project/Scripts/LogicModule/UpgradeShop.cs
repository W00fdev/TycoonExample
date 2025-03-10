﻿using _Project.Scripts.Animations;
using _Project.Scripts.Components.Buttons;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.Spawners;
using _Project.Scripts.UI.Models;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.LogicModule
{
    public class UpgradeShop : MonoBehaviour
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
            var upgrades = PersistentProgress.Instance.SpawnerUpgrades;
            int index;
            for (index = 0; index < spawnersCount; index++)
                LoadSpawner(index, index < upgrades.Count ? upgrades[index] : 0);

            if (_spawnerButtons.Length > index && _spawnerDatas.Length > index)
                EnableSpawnerButton(index);
        }

        private void LoadSpawner(int spawnerIndex, int upgradeIndex)
        {
            if (_spawnerDatas.Length <= spawnerIndex)
                return;

            var spawnerData = _spawnerDatas[spawnerIndex];
            spawnerData.Initialize(upgradeIndex);

            if (spawnerData.Index >= spawnerData.UpgradesCount)
            {
                if (_upgradeButtons[spawnerIndex].TryGetComponent(out ButtonSender sender))
                    sender.DisableButton();
            }
            else
            {
                _upgradeButtons[spawnerIndex].SetPriceInfo(spawnerData.UpgradePrice.ToHeaderMoneyFormat());
            }

            EnableUpgraderButton(spawnerIndex);
            _upgradeController.Open(spawnerData, spawnerIndex);
        }

        public void BuySpawner(int spawnerIndex)
        {
            if (_spawnerDatas.Length <= spawnerIndex)
                return;

            var spawnerData = _spawnerDatas[spawnerIndex];
            if (_currencyPipe.TrySpendCash(spawnerData.BuyPrice) == false)
                return;

            EnableUpgraderButton(spawnerIndex);
            _upgradeController.Open(spawnerData, spawnerIndex);

            int nextSpawnerLevel = spawnerIndex + 1;
            if (_spawnerButtons.Length > nextSpawnerLevel && _spawnerDatas.Length > nextSpawnerLevel)
                EnableSpawnerButton(nextSpawnerLevel);
        }

        public void TryBuyNext()
        {
            //  int spawnerIndex = _upgradeController.SpawnerLevel;
        }

        private void EnableUpgraderButton(int spawnerIndex)
        {
            _spawnerButtons[spawnerIndex].DisableSelf();
            _upgradeButtons[spawnerIndex].EnableSelf();

            var upgradePrice = _spawnerDatas[spawnerIndex].UpgradePrice;
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
            data.Initialize(0);

            _spawnerButtons[nextUpgradeLevel].gameObject.SetActive(true);
            _spawnerButtons[nextUpgradeLevel].Initialize(
                Utils.Utils.GetSpawnerKeyName(nextUpgradeLevel),
                spawnerPrice: data.BuyPrice.ToHeaderMoneyFormat(),
                speed: data.SpawnerSpeed.ToSpeedFormat(),
                productPrice: data.ProductPrice.ToString());
        }

        public void BuyUpgrade(int spawnerIndex)
        {
            var upgradeData = _spawnerDatas[spawnerIndex];
            if (_currencyPipe.TrySpendCash(upgradeData.UpgradePrice) == false)
                return;

            var nextUpgrade = _spawnerDatas[spawnerIndex].Upgrade();
            PersistentProgress.Instance.UpdateSpawnerUpgrade(spawnerIndex, _spawnerDatas[spawnerIndex].Index);

            if (nextUpgrade == null)
            {
                if (_upgradeButtons[spawnerIndex].TryGetComponent(out ButtonSender sender))
                    sender.DisableButton();
            }
            else
            {
                _upgradeButtons[spawnerIndex].SetPriceInfo(upgradeData.UpgradePrice.ToHeaderMoneyFormat());
            }
        }
    }
}