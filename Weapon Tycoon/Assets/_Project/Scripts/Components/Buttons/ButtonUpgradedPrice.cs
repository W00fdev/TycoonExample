using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class ButtonUpgradedPrice : MonoBehaviour
    {
        [SerializeField] private int _spawnerIndex;

        public void BuyUpgrade()
        {
            EventBus.BuySpawnerUpgradePricePressed?.Invoke(_spawnerIndex);
        }
    }
}