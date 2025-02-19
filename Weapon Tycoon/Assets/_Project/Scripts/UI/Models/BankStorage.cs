using System;
using Sirenix.OdinInspector;

namespace _Project.Scripts.CurrencyModule.Models
{
    [Serializable]
    public class BankStorage : Currency
    {
        public BankStorage(long money) : base(money)
        {
        }
    
        [Button("Cashout Money")]
        public long Cashout()
        {
            long temp = _money;
            _money = 0;

            return temp;
        }
    }
}