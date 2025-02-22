using _Project.Scripts.Infrastructure.ScriptableEvents.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Infrastructure.ScriptableEvents.Listeners
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventChannel<T> _eventChannel;
        [SerializeField] private UnityEvent<T> _unityEvent;

        private void Awake()
        {
            _eventChannel.Register(this);
        }

        private void OnDestroy()
        {
            _eventChannel.Unregister(this);
        }

        public void Raise(T value)
        {
            _unityEvent?.Invoke(value);
        }
    }

    public class EventListener : EventListener<Empty> {}
}