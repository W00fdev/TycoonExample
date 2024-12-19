using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class ButtonUpgradedSpeed : MonoBehaviour
    {
        [SerializeField] private int _spawnerIndex;
        
        public void BuyUpgrade()
        {
            EventBus.BuySpawnerUpgradeSpeedPressed?.Invoke(_spawnerIndex);
        }
    }
}