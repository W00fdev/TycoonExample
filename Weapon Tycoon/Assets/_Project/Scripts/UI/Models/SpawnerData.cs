using System;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [Serializable]
    public class SpawnerData
    {
        [SerializeField] private SpawnerUpgradeData _data;

        private int _index;

        public float SpawnerSpeed => _data.Upgrades[_index].Speed;
        public long ProductPrice => _data.Upgrades[_index].ProductPrice;
        public long BuyPrice => _data.Upgrades[_index].BuyPrice;

        public long UpgradePrice => _index + 1 < _data.Upgrades.Length
            ? _data.Upgrades[_index + 1].BuyPrice
            : 0;

        public int Index => _index;
        public SpawnerUpgradeData.UpgradeData UpgradeData => _data.Upgrades[_index];
        public int UpgradesCount => _data.Upgrades.Length;

        public event Action SpawnerDataChanged;

        public void Initialize(int index)
        {
            _index = index;
            SpawnerDataChanged?.Invoke();
        }

        public SpawnerUpgradeData.UpgradeData Upgrade()
        {
            var nextUpdateData = _index + 1 < _data.Upgrades.Length
                ? _data.Upgrades[++_index]
                : null;

            SpawnerDataChanged?.Invoke();
            return nextUpdateData;
        }
    }
}