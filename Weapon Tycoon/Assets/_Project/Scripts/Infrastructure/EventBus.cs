using System;
using _Project.Scripts.CurrencyModule;

namespace _Project.Scripts.Infrastructure
{
    public static class EventBus
    {
        public static event Action<IEntity> EntityConsumed;
        public static event Action BuyNextSpawnerPressed;
        
    }
}