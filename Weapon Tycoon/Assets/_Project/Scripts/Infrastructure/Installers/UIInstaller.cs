using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Presenters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class UIInstaller : MonoBehaviour
    {
        [SerializeField] private CurrencyPipe _pipe;
        
        [Space] [ShowInInspector, ReadOnly] private BankStorage _bankStorage;
        [Space] [ShowInInspector, ReadOnly] private WalletStorage _walletStorage;
        
        public void Initialize()
        {
            _bankStorage = new BankStorage(PersistentProgress.Instance.MoneyBank);
            _walletStorage = new WalletStorage(PersistentProgress.Instance.MoneyWallet);
            
            _pipe.Initialize(_bankStorage, _walletStorage);
        }
    }
}