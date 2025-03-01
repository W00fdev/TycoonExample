using System;
using _Project.Scripts.Components.Buttons;
using _Project.Scripts.Components.Spawners;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.BigBeautifulWall;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views.BigBeautifulWall;
using _Project.Scripts.UI.Views.Spawners;
using _Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.LogicModule.BigBeautifulWall
{
    public class WallUpgrader : MonoBehaviour
    {
        [SerializeField] private WallData _wallData;
        [SerializeField] private WallBuyerInfoView _wallBuyButton;
        [SerializeField] private UpgraderInfoView _upgradeButton;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private Wall _wall;
        [SerializeField] private string _wallKeyName;
        
        [Inject] private PersistentProgress _progress;

        private Action _wallBought;
        
        public void Initialize(Action wallBought)
        {
            int upgradeIndex = _progress.Data.WallUpgrades;
            _wallBought = wallBought;
            _wallData.Initialize(upgradeIndex);
            
            if (upgradeIndex == -1)
                ShowBuyButton();
            else
                OpenOrLoad(upgradeIndex);
        }

        public void OpenOrLoad(int upgradeIndex)
        {
            if (upgradeIndex > 0)
                LoadWall();
            else
                OpenWall();
        }

        private void LoadWall()
        {
            OpenWall();
            UpdateButtonViewAfterUpgrade();
        }

        public void BuyWall()
        {
            if (_currencyPipe.TrySpendCash(_wallData.BuyPrice) == false)
                return;
            
            OpenWall();
            _wallBought?.Invoke();
        }
        
        public void BuyUpgrade()
        {
            if (_currencyPipe.TrySpendCash(_wallData.UpgradePrice) == false)
                return;

            _wallData.Upgrade();
            SaveUpgradeToData();

            UpdateButtonViewAfterUpgrade();
        }
        
        public void ShowBuyButton()
        {
            _wallBuyButton.gameObject.SetActive(true);
            _wallBuyButton.Initialize(
                _wallKeyName,
                wallPrice: _wallData.BuyPrice.ToHeaderMoneyFormat(),
                regen: _wallData.Regeneration.ToString(),
                health: _wallData.Health.ToString());
        }

        private void SaveUpgradeToData() => _progress.Data.UpdateWallUpgrade(_wallData.Index);

        private void UpdateButtonViewAfterUpgrade()
        {
            if (_wallData.IsUpgradeExist() == false)
                _upgradeButton.GetComponent<ButtonSender>().DisableButton();
            else
                _upgradeButton.SetPriceInfo(_wallData.UpgradePrice.ToHeaderMoneyFormat());
        }

        private void EnableUpgraderButton()
        {
            _wallBuyButton.DisableSelf();
            _upgradeButton.EnableSelf();
            _upgradeButton.SetPriceInfo(_wallData.UpgradePrice.ToHeaderMoneyFormat());
        }

        private void OpenWall()
        {
            EnableUpgraderButton();
            SaveUpgradeToData();
            
            _wall.gameObject.SetActive(true);
            _wall.Initialize(_wallData);
        }
    }
    
    
}