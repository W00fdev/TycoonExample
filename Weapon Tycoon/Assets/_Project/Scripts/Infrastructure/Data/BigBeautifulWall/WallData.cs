using System;
using _Project.Scripts.Infrastructure.Data.Spawners;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Infrastructure.Data.BigBeautifulWall
{
    [Serializable]
    public class WallData
    {
        [SerializeField] private WallUpgradeConfig _config;
        private int _index;

        public int Regeneration => _config.Upgrades[_index].Regeneration;
        public long Health => _config.Upgrades[_index].Health;
        public long BuyPrice => _config.Upgrades[_index].BuyPrice;

        public long UpgradePrice => _index + 1 < UpgradesCount
            ? _config.Upgrades[_index + 1].BuyPrice
            : 0;

        public int Index => _index;
        public WallUpgradeConfig.UpgradeData UpgradeData => _config.Upgrades[_index];
        public int UpgradesCount => _config.Upgrades.Length;

        public event Action SpawnerDataChanged;

        public void Initialize(int index)
        {
            _index = index;
            SpawnerDataChanged?.Invoke();
        }

        public WallUpgradeConfig.UpgradeData Upgrade()
        {
            var nextUpdateData = IsUpgradeExist()
                ? _config.Upgrades[++_index]
                : null;

            SpawnerDataChanged?.Invoke();
            return nextUpdateData;
        }

        public bool IsUpgradeExist()
            => _index + 1 < UpgradesCount;
    }
}