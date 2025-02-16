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

        public void Cashout(int bankMoney) 
            => _walletPresenter.AddCash(bankMoney);

        public bool SpentCash(int amount)
            => _walletPresenter.SpentCash(amount);

        private void BankIncome(int income) 
            => _bankPresenter.BankIncome(income);
    }
}