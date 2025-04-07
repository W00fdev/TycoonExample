using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using UnityEngine;

namespace _Project.Scripts.Components.Buttons
{
    public abstract class BaseButtonSender<T, TX> : MonoBehaviour
        where T : EventChannel<TX>
    {
        protected bool _isDisabled;

        public void DisableButton()
        {
            _isDisabled = true;
            gameObject.SetActive(false);
        }
    }
}