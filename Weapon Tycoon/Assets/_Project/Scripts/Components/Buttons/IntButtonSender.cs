using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using UnityEngine;

namespace _Project.Scripts.Components.Buttons
{
    public class IntButtonSender : BaseButtonSender<IntEventChannel, int>
    {
        [SerializeField] private int _dataSend;
        [SerializeField] private IntEventChannel _eventChannel;
        
        // Context Invocation
        public void Send()
        {
            if (_isDisabled)
                return;
              
            _eventChannel.Invoke(_dataSend);
        }
    }
}