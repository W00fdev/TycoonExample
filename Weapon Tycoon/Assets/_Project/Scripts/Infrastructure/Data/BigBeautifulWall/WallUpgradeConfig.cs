using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Data.BigBeautifulWall
{
    [CreateAssetMenu(fileName = "Wall Upgrade Config", menuName = "Config/Wall Upgrades")]
    public class WallUpgradeConfig : ScriptableObject
    {
        [SerializeField] private UpgradeData[] _upgrades;
        public UpgradeData[] Upgrades => _upgrades;
        
        [Serializable]
        public class UpgradeData
        {
            public int Regeneration;
            public int Health;
            public long BuyPrice;
        }
    }
}