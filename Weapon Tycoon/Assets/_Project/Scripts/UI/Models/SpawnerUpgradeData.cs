using System;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [CreateAssetMenu(fileName = "Spawner Upgrade Data", menuName = "Config/SpawnersUpgrades")]
    public class SpawnerUpgradeData : ScriptableObject
    {
        [SerializeField] private SpeedUpgradeData[] _speedUpgrades;
        [SerializeField] private PriceUpgradeData[] _priceUpgrades;

        public SpeedUpgradeData[] SpeedUgrades => _speedUpgrades;
        public PriceUpgradeData[] PriceUgrades => _priceUpgrades;
        
        [Serializable]
        public class SpeedUpgradeData
        {
            public float Speed;
            public int BuyPrice;
        }
        
        [Serializable]
        public class PriceUpgradeData
        {
            public int ProductPrice;
            public int BuyPrice;
        }
        

    }
}