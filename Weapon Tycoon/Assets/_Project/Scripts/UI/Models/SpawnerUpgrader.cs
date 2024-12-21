using System;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [Serializable]
    public class SpawnerUpgrader
    {
        [SerializeField] private SpawnerUpgradeData _data;
            
        private int _speedUpgradeIndex;
        private int _priceUpgradeIndex;
        
        public SpawnerUpgradeData.SpeedUpgradeData SpeedUpgrade => _data.SpeedUgrades[_speedUpgradeIndex];
        public SpawnerUpgradeData.PriceUpgradeData PriceUpgrade => _data.PriceUgrades[_priceUpgradeIndex];
        
        public SpawnerUpgradeData.SpeedUpgradeData NextSpeedUpgrade()
        {
            return (_speedUpgradeIndex + 1 < _data.SpeedUgrades.Length) 
                ? _data.SpeedUgrades[++_speedUpgradeIndex] 
                : null;
        }
        
        public SpawnerUpgradeData.PriceUpgradeData NextPriceUpgrade()
        {
            return (_priceUpgradeIndex + 1 < _data.PriceUgrades.Length) 
                ? _data.PriceUgrades[++_priceUpgradeIndex] 
                : null;
        }
    }
}