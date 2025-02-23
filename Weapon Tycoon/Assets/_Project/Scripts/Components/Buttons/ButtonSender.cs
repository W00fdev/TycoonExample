using System.Collections.Generic;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.ScriptableEvents;
using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using UnityEngine;

namespace _Project.Scripts.Components.Buttons
{
    public class ButtonSender : MonoBehaviour
    {
        [SerializeField] private int _dataSend;
        [SerializeField] private IntEventChannel _eventChannel;
        private bool _isDisabled;
        
        // Context Invocation
        public void Send()
        {
            if (_isDisabled)
                return;
              
            _eventChannel.Invoke(_dataSend);
        }

        public void DisableButton()
        {
            _isDisabled = true;
            gameObject.SetActive(false);
        }
    }
}