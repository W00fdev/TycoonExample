using System;
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Spawners;
        public List<int> SpawnerUpgrades = new();

        public long MoneyWallet;
        public long MoneyBank;

        public void UpdateSpawnerUpgrade(int index, int upgradeIndex)
        {
            if (index < SpawnerUpgrades.Count)
                SpawnerUpgrades[index] = upgradeIndex;
            else
                SpawnerUpgrades.Add(upgradeIndex);
        }
    }
}