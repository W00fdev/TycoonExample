using System;
using System.Collections.Generic;
using _Project.Scripts.UI.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Infrastructure.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Spawners;
        public List<SpawnerUpgradeDTO> SpawnerUpgrades = new();

        public long MoneyWallet;
        public long MoneyBank;
        
        public void UpdateSpawnerUpgrade(SpawnerUpgradeDTO dto, int index)
        {
            if (index < SpawnerUpgrades.Count)
                SpawnerUpgrades[index] = dto;
            else
                SpawnerUpgrades.Add(dto);
        }
    }
}