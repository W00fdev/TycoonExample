using System.Collections.Generic;
using _Project.Scripts.Infrastructure.ScriptableEvents.Listeners;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.ScriptableEvents.Channels
{
    [CreateAssetMenu(menuName = "Events/EventChannel")]
    public class EventChannel : EventChannel<Empty> {}
    
    public class EventChannel<T> : ScriptableObject
    {
        private readonly HashSet<EventListener<T>> _observers = new();

        public void Invoke(T value)
        {
            foreach (var observer in _observers)
            {
                observer.Raise(value);
            }
        }

        public void Register(EventListener<T> observer)
            => _observers.Add(observer);

        public void Unregister(EventListener<T> observer)
            => _observers.Remove(observer);
    }
}