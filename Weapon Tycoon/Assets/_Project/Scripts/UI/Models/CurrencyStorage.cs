using Sirenix.OdinInspector;

namespace _Project.Scripts.CurrencyModule.Models
{
    public abstract class Currency
    {
        protected long _money;
        public long Money => _money;

        public Currency(long money)
        {
            _money = money;
        }
    
        [Button( "Add Money")]
        public void AddCurrency(long amount)
        {
            _money += amount;
        }
    }
}