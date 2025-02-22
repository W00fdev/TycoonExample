using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.CurrencyModule.Presenters;
using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Scripts.UI.Presenters
{
    public class CurrencyPipe : MonoBehaviour
    {
        [SerializeField] private BankPresenter _bankPresenter;
        [SerializeField] private WalletPresenter _walletPresenter;

        public void Initialize(BankStorage bankStorage,
            WalletStorage walletStorage)
        {
            _bankPresenter.Initialize(bankStorage, this);
            _walletPresenter.Initialize(walletStorage, this);
            
            EventBus.BankIncome += BankIncome;
        }

        public void Cashout(long bankMoney) 
            => _walletPresenter.AddCash(bankMoney);

        public bool TrySpendCash(long amount)
            => _walletPresenter.TrySpendCash(amount);

        private void BankIncome(long income) 
            => _bankPresenter.BankIncome(income);
    }
}