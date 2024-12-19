using Sirenix.OdinInspector;

namespace _Project.Scripts.CurrencyModule.Models
{
    public abstract class Currency
    {
        protected int _money;
        public int Money => _money;

        public Currency(int money)
        {
            _money = money;
        }
    
        [Button( "Add Money")]
        public void AddCurrency(int amount)
        {
            _money += amount;
        }
    }
}