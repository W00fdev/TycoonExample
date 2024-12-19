using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.CurrencyModule.Models
{
    [CreateAssetMenu(fileName = "Spawner", menuName = "Config/Spawners")]
    public class SpawnerData : ScriptableObject
    {
        public string SpawnerName;
        public ValuableEntity Product;
        public float SpawnerSpeed;
        public int BuyPrice;

        public Action SpawnerDataChanged;
        
        [ShowInInspector, ReadOnly]
        public int ProductPrice
        {
            get => Product.Price;
            set => Product.Price = value;
        }
    }
}