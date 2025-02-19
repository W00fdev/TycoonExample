using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI.Models
{
    [CreateAssetMenu(fileName = "Spawner Upgrade Data", menuName = "Config/SpawnersUpgrades")]
    public class SpawnerUpgradeData : ScriptableObject
    {
        [SerializeField] private UpgradeData[] _upgrades;
        public UpgradeData[] Upgrades => _upgrades;
        
        [Serializable]
        public class UpgradeData
        {
            public float Speed;
            public int ProductPrice;
            public int BuyPrice;
        }
    }
}