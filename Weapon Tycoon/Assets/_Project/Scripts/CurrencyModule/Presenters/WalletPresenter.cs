using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Views;
using UnityEngine;

namespace _Project.Scripts.CurrencyModule.Presenters
{
    public class WalletPresenter : MonoBehaviour
    {
        [SerializeField] private WalletView _view;

        private CurrencyPipe _pipe;
        private WalletStorage _storage;
        
        public void Initialize(WalletStorage storage, CurrencyPipe pipe)
        {
            _storage = storage;
            UpdateView();
        }

        public void AddCash(int amount)
        {
            _storage.AddCurrency(amount);
            UpdateView();
        }

        public bool SpentCash(int amount)
        {
            if (_storage.SpendMoney(amount))
            {
                UpdateView();
                return true;
            }

            return false;
        }
        
        private void UpdateView() => _view.UpdateCurrency(_storage.Money.ToString());
    }
}