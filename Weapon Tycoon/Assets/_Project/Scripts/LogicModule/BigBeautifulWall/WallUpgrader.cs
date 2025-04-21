using System;
using _Project.Scripts.Animations;
using _Project.Scripts.Components.Buttons;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.BigBeautifulWall;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.UI.Views.BigBeautifulWall;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.LogicModule.BigBeautifulWall
{
    public class WallUpgrader : MonoBehaviour
    {
        [SerializeField] private WallData _wallData;
        [SerializeField] private WallBuyerInfoView _wallBuyButton;
        [SerializeField] private UpgraderInfoView _upgradeButton;
        [SerializeField] private ButtonAnimation _upgradeButtonAnimation;
        [SerializeField] private RepairInfoView _repairButton;
        [SerializeField] private ButtonAnimation _repairButtonAnimation;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private Wall _wall;
        [SerializeField] private string _wallKeyName;

        private PersistentProgress _progress;
        private long _repairPrice;

        public Wall Wall => _wall;
        public event Action WallOpened;
        
        public void Initialize(PersistentProgress progress)
        {
            _progress = progress;
            
            int upgradeIndex = _progress.Data.WallUpgrades;
            _wallData.Initialize(upgradeIndex == -1 ? 0 : upgradeIndex);
            
            if (upgradeIndex == -1)
                ShowBuyButton();
            else
                OpenWall();
        }
        
        public void LoadWall()
        {
            if (_wall.IsInitialized == false || _progress.Data.WallUpgrades < 0)
                return; 
            
            _wall.LoadData(_wallData, _progress.Data);
            UpdateButtonViewAfterUpgrade();
        }

        public void BuyWall()
        {
            if (_currencyPipe.TrySpendCash(_wallData.BuyPrice) == false)
                return;
            
            OpenWall();
        }

        public void BuyRepair()
        {
            if (_currencyPipe.TrySpendCash(_wallData.RepairPrice) == false)
                return;

            _wall.Repair();
        }
        
        public void BuyUpgrade()
        {
            if (_currencyPipe.TrySpendCash(_wallData.UpgradePrice) == false)
                return;

            _wallData.Upgrade();
            SaveUpgradeToData();

            UpdateButtonViewAfterUpgrade();
        }

        private void ShowBuyButton()
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
                _upgradeButton.GetComponent<IntButtonSender>().DisableButton();
            else
                _upgradeButton.SetPriceInfo(_wallData.UpgradePrice.ToHeaderMoneyFormat());
            
            _repairButton.SetPriceInfo(_wallData.RepairPrice.ToHeaderMoneyFormat());
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
            
            _wall.Health.ChangedHealthEvent += ChangeButtonsVisibility;
            WallOpened?.Invoke();
        }

        private void ChangeButtonsVisibility(int health, int maxHealth)
        {
            if (health == maxHealth)
            {
                _repairButtonAnimation.SetOnReleaseCallback(_repairButton.DisableSelf);
                _repairButtonAnimation.ReleaseButton();
            }
            else
            {
                _repairButton.EnableSelf();

                if (health == 0)
                {
                    _upgradeButtonAnimation.SetOnReleaseCallback(_upgradeButton.DisableSelf);
                    _upgradeButtonAnimation.ReleaseButton();
                }
                else if (_wallData.IsUpgradeExist())
                {
                    _upgradeButton.EnableSelf();
                }
            }
        }

        private void OnDestroy()
        {
            if (_wall != null && _wall.Health != null)
                _wall.Health.ChangedHealthEvent -= ChangeButtonsVisibility;
        }
    }
}