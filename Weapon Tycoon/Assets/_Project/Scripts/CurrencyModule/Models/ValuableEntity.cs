using _Project.Scripts.Models;
using UnityEngine;

namespace _Project.Scripts.CurrencyModule.Models
{
    [CreateAssetMenu(fileName = "Valuable Entity", menuName = "Config/Valuable Entities")]
    public class ValuableEntity : ScriptableObject
    {
        public int Price;
        public RarityType Rarity;
    }
}