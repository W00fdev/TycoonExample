using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.CurrencyModule.Presenters
{
    public class BankPresenter : MonoBehaviour
    {
        [SerializeField] private BankView _view;
        [SerializeField] private CashStacksView _cashStacksView;

        private CurrencyPipe _pipe;
        private BankStorage _storage;
        
        public void Initialize(BankStorage storage, 
            CurrencyPipe pipe)
        {
            _storage = storage;
            _pipe = pipe;

            UpdateView();
        }

        private void UpdateView()
        {
            _view.UpdateCurrency($"$ {_storage.Money.ToString()}");
            _cashStacksView.UpdateCurrency(_storage.Money);
        }

        public void ProductConsumed(IEntity entity)
        {
            _storage.AddCurrency(entity.Entity.Price);
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