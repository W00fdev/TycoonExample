using System;
using _Project.Scripts.CurrencyModule;

namespace _Project.Scripts.Infrastructure
{
    public static class EventBus
    {
        public static Action<IEntity> EntityConsumed;
        public static Action<int> BankIncome;
        public static Action BuyNextSpawnerPressed;
        public static Action<int> BuySpawnerUpgradeSpeedPressed;
        public static Action<int> BuySpawnerUpgradePricePressed;
    }
}