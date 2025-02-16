using System;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [Serializable]
    public class SpawnerData
    {
        [SerializeField] private SpawnerConfig _config;

        [HideInInspector] public float SpawnerSpeed;
        [HideInInspector] public int ProductPrice;
        
        public Action SpawnerDataChanged;

        public int BuyPrice => _config.BuyPrice;
        public string SpawnerName => _config.SpawnerName;
        
        public void Initialize()
        {
            SpawnerSpeed = _config.SpawnerSpeed;
            ProductPrice = _config.ProductPrice;
        }
    }
}