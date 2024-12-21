using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Scripts.Components.Buttons
{
    public class ButtonUpgradedSpeed : MonoBehaviour, IButtonUpgrader
    {
        [SerializeField] private int _spawnerIndex;
        private bool _isDisabled;

        public void BuyUpgrade()
        {
            if (_isDisabled)
                return;
            
            EventBus.BuySpawnerUpgradeSpeedPressed?.Invoke(_spawnerIndex);
        }

        public void DisableButton()
        {
            _isDisabled = true;
        }
    }
}