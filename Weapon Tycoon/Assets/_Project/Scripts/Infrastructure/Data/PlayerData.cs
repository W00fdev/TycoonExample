using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _Project.Scripts.Infrastructure.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Spawners;
        public int Turrets;
        public List<int> SpawnerUpgrades = new();
        public List<int> TurretUpgrades = new();

        public long MoneyWallet;
        public long MoneyBank;

        public PlayerData()
        {
        }
        
        public PlayerData(PlayerData data)
        {
            Spawners = data.Spawners;
            Turrets = data.Turrets;
            SpawnerUpgrades = data.SpawnerUpgrades;
            TurretUpgrades = data.TurretUpgrades;
            MoneyWallet = data.MoneyWallet;
            MoneyBank = data.MoneyBank;
        }
        
        public void UpdateSpawnerUpgrade(int index, int upgradeIndex)
        {
            if (index < SpawnerUpgrades.Count)
                SpawnerUpgrades[index] = upgradeIndex;
            else
                SpawnerUpgrades.Add(upgradeIndex);
        }
        
        public void UpdateTurretUpgrade(int index, int upgradeIndex)
        {
            if (index < TurretUpgrades.Count)
                TurretUpgrades[index] = upgradeIndex;
            else
                TurretUpgrades.Add(upgradeIndex);
        }

        public int GetSpawnerUpgradeLevelOrDefault(int spawnerIndex) =>
            (spawnerIndex < SpawnerUpgrades.Count) 
                ? SpawnerUpgrades[spawnerIndex] 
                : 0;
        
        public int GetTurretUpgradeLevelOrDefault(int turretIndex) =>
            (turretIndex < TurretUpgrades.Count) 
                ? TurretUpgrades[turretIndex] 
                : 0;
    }
}