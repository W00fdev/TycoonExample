using System;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [Serializable]
    public class SpawnerUpgrader
    {
        [SerializeField] private SpawnerUpgradeData _data;
            
        private int _upgradeIndex;
        
        public SpawnerUpgradeData.UpgradeData Upgrade => _data.Upgrades[_upgradeIndex];
        
        public SpawnerUpgradeData.UpgradeData NextUpgrade()
        {
            return (_upgradeIndex + 1 < _data.Upgrades.Length) 
                ? _data.Upgrades[++_upgradeIndex] 
                : null;
        }
    }
}