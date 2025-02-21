using System;
using _Project.Scripts.CurrencyModule;

namespace _Project.Scripts.Infrastructure
{
    public static class EventBus
    {
        public static Action<long> BankIncome;
        public static Action BuyNextSpawnerPressed;
        public static Action<int> BuySpawnerUpgradePressed;
    }
}