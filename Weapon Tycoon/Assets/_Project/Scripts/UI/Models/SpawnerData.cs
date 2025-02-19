using System;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [Serializable]
    public class SpawnerUpgradeDTO
    {
        public float SpawnerSpeed;
        public long ProductPrice;
        public int UpgradeIndex;

        public SpawnerUpgradeDTO(float spawnerSpeed, long productPrice, int upgradeIndex)
        {
            SpawnerSpeed = spawnerSpeed;
            ProductPrice = productPrice;
            UpgradeIndex = upgradeIndex;
        }
    }
    
    [Serializable]
    public class SpawnerData
    {
        [SerializeField] private SpawnerUpgradeData _data;

        private float _spawnerSpeed;
        private long _productPrice;
        private int _upgradeIndex;
        private long _upgradePrice;
        
        public float SpawnerSpeed => _spawnerSpeed;
        public long ProductPrice => _productPrice;
        public long NextUpgradePrice => _upgradeIndex + 1 < _data.Upgrades.Length 
            ? _data.Upgrades[_upgradeIndex + 1].BuyPrice 
            : 0;

        public long BuyPrice => _data.Upgrades[_upgradeIndex].BuyPrice;
        
        public int UpgradeIndex => _upgradeIndex;
        public int TotalUpgradesCount => _data.Upgrades.Length;
        
        public SpawnerUpgradeDTO DTO
            => new SpawnerUpgradeDTO(_spawnerSpeed, _productPrice, _upgradeIndex);
        
        public event Action SpawnerDataChanged;
        
        public SpawnerUpgradeData.UpgradeData Upgrade => _data.Upgrades[_upgradeIndex];
        
        public void Initialize()
        {
            _upgradeIndex = 0;
            _spawnerSpeed = _data.Upgrades[0].Speed;
            _productPrice = _data.Upgrades[0].ProductPrice;
            _upgradePrice = NextUpgradePrice;
        }

        public void SetData(SpawnerUpgradeDTO dto)
        {
            _upgradeIndex = dto.UpgradeIndex;
            _spawnerSpeed = dto.SpawnerSpeed;
            _productPrice = dto.ProductPrice;
            
            _upgradePrice = NextUpgradePrice;
            SpawnerDataChanged?.Invoke();
        }
        
        public SpawnerUpgradeData.UpgradeData ApplyNextUpgrade()
        {
            var nextUpdateData =  (_upgradeIndex + 1 < _data.Upgrades.Length) 
                ? _data.Upgrades[++_upgradeIndex] 
                : null;
         
            _productPrice = _data.Upgrades[_upgradeIndex].ProductPrice;
            _spawnerSpeed = _data.Upgrades[_upgradeIndex].Speed;
            _upgradePrice = NextUpgradePrice;
            
            SpawnerDataChanged?.Invoke();

            return nextUpdateData;
        }
    }
}