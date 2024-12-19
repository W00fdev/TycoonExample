using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.CurrencyModule.Models
{
    [CreateAssetMenu(fileName = "Spawner", menuName = "Config/Spawners")]
    public class SpawnerData : ScriptableObject
    {
        public ValuableEntity Product;
        public float Speed;
        public int BuyPrice;

        [field: SerializeField, ReadOnly] public int StartPrice => Product.Price;
    }
}