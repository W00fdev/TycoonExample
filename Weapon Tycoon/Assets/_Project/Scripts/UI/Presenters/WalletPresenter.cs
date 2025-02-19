using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
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

        public void AddCash(long amount)
        {
            _storage.AddCurrency(amount);
            UpdateView();
        }

        public bool SpentCash(long amount)
        {
            if (_storage.SpendMoney(amount))
            {
                UpdateView();
                
                return true;
            }

            return false;
        }
        
        private void UpdateView()
        {
            _view.UpdateCurrency(_storage.Money.ToHeaderMoneyFormat());
            PersistentProgress.Instance.MoneyWallet = _storage.Money;
        }
    }
}