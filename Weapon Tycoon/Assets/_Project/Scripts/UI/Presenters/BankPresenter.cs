using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Presenters;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CurrencyModule.Presenters
{
    public class BankPresenter : MonoBehaviour
    {
        [SerializeField] private BankView _view;
        [SerializeField] private CashStacksView _cashStacksView;

        private CurrencyPipe _pipe;
        private BankStorage _storage;
        
        [Inject] private PersistentProgress _progress;
        
        public void Initialize(BankStorage storage, 
            CurrencyPipe pipe)
        {
            _storage = storage;
            _pipe = pipe;

            UpdateView();
        }

        private void UpdateView()
        {
            _view.UpdateCurrency(_storage.Money.ToHeaderMoneyFormat());
            _cashStacksView.UpdateCurrency(_storage.Money);

            _progress.Data.MoneyBank = _storage.Money;
        }

        public void BankIncome(long income)
        {
            _storage.AddCurrency(income);
            UpdateView();
        }

        [Button("Cashout")]
        public void Cashout()
        {
            _pipe.Cashout(_storage.Cashout());
            UpdateView();
        }
    }
}