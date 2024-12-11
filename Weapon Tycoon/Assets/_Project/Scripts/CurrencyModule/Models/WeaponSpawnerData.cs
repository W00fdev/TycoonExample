using UnityEngine;

namespace _Project.Scripts.CurrencyModule.Models
{
    [CreateAssetMenu(fileName = "Spawner", menuName = "Config/Spawners")]
    public class WeaponSpawnerData : ScriptableObject
    {
        public ValuableEntity Product;
        public float Speed;
        public int BuyPrice;
    }
}