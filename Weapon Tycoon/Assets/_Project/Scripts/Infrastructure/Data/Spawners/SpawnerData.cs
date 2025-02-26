using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Infrastructure.Data.Spawners
{
    [Serializable]
    public class SpawnerData
    {
        [FormerlySerializedAs("_data")] 
        [SerializeField] private SpawnerUpgradeConfig _config;

        private int _index;

        public float SpawnerSpeed => _config.Upgrades[_index].Speed;
        public long ProductPrice => _config.Upgrades[_index].ProductPrice;
        public long BuyPrice => _config.Upgrades[_index].BuyPrice;

        public long UpgradePrice => _index + 1 < UpgradesCount
            ? _config.Upgrades[_index + 1].BuyPrice
            : 0;

        public int Index => _index;
        public SpawnerUpgradeConfig.UpgradeData UpgradeData => _config.Upgrades[_index];
        public int UpgradesCount => _config.Upgrades.Length;

        public event Action SpawnerDataChanged;

        public void Initialize(int index)
        {
            _index = index;
            SpawnerDataChanged?.Invoke();
        }

        public SpawnerUpgradeConfig.UpgradeData Upgrade()
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