using System;
using _Project.Scripts.Infrastructure.Data.Spawners;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Data.Turrets
{
    [Serializable]
    public class TurretData
    {
        [SerializeField] private TurretUpgradeConfig _config;
        private int _index;

        public float RPM => _config.Upgrades[_index].RPM;
        public long Damage => _config.Upgrades[_index].Damage;
        public long BuyPrice => _config.Upgrades[_index].BuyPrice;

        public long UpgradePrice => _index + 1 < _config.Upgrades.Length
            ? _config.Upgrades[_index + 1].BuyPrice
            : 0;

        public int Index => _index;
        public TurretUpgradeConfig.TurretStat UpgradeData => _config.Upgrades[_index];
        public int UpgradesCount => _config.Upgrades.Length;

        public event Action TurretDataChanged;

        public void Initialize(int index)
        {
            _index = index;
            TurretDataChanged?.Invoke();
        }

        public TurretUpgradeConfig.TurretStat Upgrade()
        {
            var nextUpdateData = _index + 1 < _config.Upgrades.Length
                ? _config.Upgrades[++_index]
                : null;

            TurretDataChanged?.Invoke();
            return nextUpdateData;
        }
    }
}