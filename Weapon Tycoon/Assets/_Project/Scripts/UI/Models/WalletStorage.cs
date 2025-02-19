using System;
using Sirenix.OdinInspector;

namespace _Project.Scripts.CurrencyModule.Models
{
    [Serializable]
    public class WalletStorage : Currency
    {
        public WalletStorage(long money) : base(money)
        {
        }
    
        [Button("Spend Money")]
        public bool SpendMoney(long amount)
        {
            if (_money >= amount)
            {
                _money -= amount;
                return true;
            }

            return false;
        }
    }
}