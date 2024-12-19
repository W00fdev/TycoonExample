﻿using System;
using System.Collections.Generic;
using _Project.Scripts.Animations;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Presenters;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public sealed class UpgradeBuyer : MonoBehaviour
    {
        [SerializeField] private List<SpawnerData> _spawnerDatas;
        [SerializeField] private List<SpawnerBuyerInfoView> _spawnerButtons;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private UpgradeController _upgradeController;

        private void Start()
        {
            EnableSpawnerButton(0);
        }

        private void OnEnable()
        {
            EventBus.BuyNextSpawnerPressed += TryBuyNext;
        }
        
        private void OnDisable()
        {
            EventBus.BuyNextSpawnerPressed -= TryBuyNext;
        }

        public void TryBuyNext()
        {
            int upgradeLevel = _upgradeController.UpgradeLevel;
            
            if (_spawnerDatas.Count <= upgradeLevel)
                return;
            
            var spawnerData = _spawnerDatas[upgradeLevel];
            if (_currencyPipe.SpentCash(spawnerData.BuyPrice) == false)
                return;
            
            //SetDisableSpawnerButton(upgradeLevel);
            _spawnerButtons[upgradeLevel].gameObject.SetActive(false);
            _upgradeController.Next(spawnerData);
            
            int nextUpgradeLevel = _upgradeController.UpgradeLevel;
            if (_spawnerButtons.Count > nextUpgradeLevel && _spawnerDatas.Count > nextUpgradeLevel)
                EnableSpawnerButton(nextUpgradeLevel);
        }

        private void SetDisableSpawnerButton(int upgradeLevel)
        {
            _spawnerButtons[upgradeLevel].GetComponent<ButtonAnimation>().SetOnReleaseCallback(
                () => _spawnerButtons[upgradeLevel].gameObject.SetActive(false));
        }

        private void EnableSpawnerButton(int nextUpgradeLevel)
        {
            var data = _spawnerDatas[nextUpgradeLevel];
            _spawnerButtons[nextUpgradeLevel].gameObject.SetActive(true);
            _spawnerButtons[nextUpgradeLevel].Initialize(
                spawnerName:    data.SpawnerName,
                spawnerPrice:   data.ProductPrice.ToHeaderMoneyFormat(),
                speed:          data.SpawnerSpeed.ToSpeedFormat(),
                productPrice:   data.ProductPrice.ToString());
        }
    }
}