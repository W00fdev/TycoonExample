using System;
using _Project.Scripts.Components.Buttons;
using _Project.Scripts.Components.Turrets;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Data.Turrets;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views.Spawners;
using _Project.Scripts.UI.Views.Turrets;
using _Project.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule.Turrets
{
    public class TurretUpgrader : MonoBehaviour
    {
        [SerializeField] private TurretData _turretData;
        [SerializeField] private TurretBuyerInfoView _buyButton;
        [SerializeField] private UpgraderInfoView _upgradeButton;
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private Turret _turret;
        [SerializeField] private int _turretIndex;
        
        [Inject] private PersistentProgress _progress;

        private Action _turretBought;
        
        public void Initialize(Action turretBought, int upgradeIndex)
        {
            _turretBought = turretBought;
            _turretData.Initialize(upgradeIndex);
        }
        
        public void OpenOrLoad(int upgradeIndex)
        {
            if (upgradeIndex > 0)
                LoadTurret();
            else
                OpenTurret();
        }

        private void LoadTurret()
        {
            OpenTurret();
            UpdateButtonViewAfterUpgrade();
        }

        public void BuyTurret()
        {
            if (_currencyPipe.TrySpendCash(_turretData.BuyPrice) == false)
                return;
            
            OpenTurret();
            
            _turretBought?.Invoke();
        }
        
        public void BuyUpgrade()
        {
            if (_currencyPipe.TrySpendCash(_turretData.UpgradePrice) == false)
                return;

            _turretData.Upgrade();
            SaveUpdateToData();

            UpdateButtonViewAfterUpgrade();
        }
        
        public void ShowBuyButton()
        {
            _buyButton.gameObject.SetActive(true);
            _buyButton.Initialize(
                Utils.Utils.GetSpawnerKeyName(_turretIndex),
                turretPrice: _turretData.BuyPrice.ToHeaderMoneyFormat(),
                damage: _turretData.Damage.ToString(),
                fireRate: _turretData.RPM.ToString());
        }

        private void SaveUpdateToData() => _progress.Data.UpdateTurretUpgrade(_turretIndex, _turretData.Index);
        
        private void UpdateButtonViewAfterUpgrade()
        {
            if (_turretData.IsUpgradeExist() == false)
                _upgradeButton.GetComponent<ButtonSender>().DisableButton();
            else
                _upgradeButton.SetPriceInfo(_turretData.UpgradePrice.ToHeaderMoneyFormat());
        }

        private void EnableUpgraderButton()
        {
            _buyButton.DisableSelf();
            _upgradeButton.EnableSelf();
            _upgradeButton.SetPriceInfo(_turretData.UpgradePrice.ToHeaderMoneyFormat());
        }

        private void OpenTurret()
        {
            EnableUpgraderButton();
            SaveUpdateToData();
            
            _turret.gameObject.SetActive(true);
            _turret.Initialize(_turretData);
            _turret.Resolve();
        }
    }
}