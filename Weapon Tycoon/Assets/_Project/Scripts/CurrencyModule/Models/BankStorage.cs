using System;
using Sirenix.OdinInspector;

namespace _Project.Scripts.CurrencyModule.Models
{
    [Serializable]
    public class BankStorage : Currency
    {
        public BankStorage(int money) : base(money)
        {
        }
    
        [Button("Cashout Money")]
        public int Cashout()
        {
            int temp = _money;
            _money = 0;

            return temp;
        }
    }
}