using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Scripts.Components.Buttons
{
    public class ButtonUpgradedPrice : MonoBehaviour, IButtonUpgrader
    {
        [SerializeField] private int _spawnerIndex;
        private bool _isDisabled;
        
        // Context Invocation
        public void BuyUpgrade()
        {
            if (_isDisabled)
                return;
                
            EventBus.BuySpawnerUpgradePricePressed?.Invoke(_spawnerIndex);
        }

        public void DisableButton()
        {
            _isDisabled = true;
            gameObject.SetActive(false);
        }
    }
}