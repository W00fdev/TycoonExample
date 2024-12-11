using System;
using _Project.Scripts.CurrencyModule.Models;
using UnityEngine;

namespace _Project.Scripts.CurrencyModule.Presenters
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
        }

        public void ProductConsumed(IEntity entity) 
            => _bankPresenter.ProductConsumed(entity);

        public void Cashout(int bankMoney) 
            => _walletPresenter.AddCash(bankMoney);

        public bool SpentCash(int amount)
            => _walletPresenter.SpentCash(amount);
    }
}