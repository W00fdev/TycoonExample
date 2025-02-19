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
        public List<SpawnerUpgradeDTO> SpawnerUpgrade = new();

        public long MoneyWallet;
        public long MoneyBank;

        public void UpdateSpawnerUpgrade(SpawnerUpgradeDTO dto, int index)
        {
            if (index < SpawnerUpgrade.Count)
                SpawnerUpgrade[index] = dto;
            else
                SpawnerUpgrade.Add(dto);
        }
    }
}