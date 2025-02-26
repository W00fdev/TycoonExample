using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Data.Turrets
{
    [CreateAssetMenu(fileName = "Turret Upgrade Data", menuName = "Config/TurretUpgrades")]
    public class TurretUpgradeConfig : ScriptableObject
    {
        [SerializeField] private TurretStat[] _upgrades;
        public TurretStat[] Upgrades => _upgrades;
        
        [Serializable]
        public class TurretStat
        {
            public int Damage;
            public int RPM;
            public long BuyPrice;
        }
    }
}