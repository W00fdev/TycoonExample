using System;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [Serializable]
    public class SpawnerData
    {
        [SerializeField] private SpawnerConfig _config;

        private float _spawnerSpeed;
        private int _productPrice;
        
        public float SpawnerSpeed => _spawnerSpeed;
        public int ProductPrice => _productPrice;
        
        public event Action SpawnerDataChanged;

        public int BuyPrice => _config.BuyPrice;
        public string SpawnerName => _config.SpawnerName;
        
        public void Initialize()
        {
            _spawnerSpeed = _config.SpawnerSpeed;
            _productPrice = _config.ProductPrice;
        }

        public void UpdateSpeedAndPrice(float speed, int price)
        {
            _spawnerSpeed = speed;
            _productPrice = price;
            SpawnerDataChanged?.Invoke();
        }
        
        public void UpdateSpeed(float speed)
        {
            _spawnerSpeed = speed;
            SpawnerDataChanged?.Invoke();
        }
        
        public void UpdatePrice(int price)
        {
            _productPrice = price;
            SpawnerDataChanged?.Invoke();
        }
    }
}