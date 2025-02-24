using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Data.Spawners
{
    [CreateAssetMenu(fileName = "Spawner Upgrade Data", menuName = "Config/SpawnersUpgrades")]
    public class SpawnerUpgradeConfig : ScriptableObject
    {
        [SerializeField] private UpgradeData[] _upgrades;
        public UpgradeData[] Upgrades => _upgrades;
        
        [Serializable]
        public class UpgradeData
        {
            public float Speed;
            public long ProductPrice;
            public long BuyPrice;
        }
    }
}