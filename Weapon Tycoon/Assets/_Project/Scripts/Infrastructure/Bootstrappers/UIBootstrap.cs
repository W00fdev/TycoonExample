using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Presenters;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstrappers
{
    public class UIBootstrap : MonoBehaviour
    {
        [SerializeField] private CurrencyPipe _pipe;
        
        [Space] [ShowInInspector, ReadOnly] private BankStorage _bankStorage;
        [Space] [ShowInInspector, ReadOnly] private WalletStorage _walletStorage;

        [Inject] private PersistentProgress _progress;
        
        public void Initialize()
        {
            _bankStorage = new BankStorage(_progress.Data.MoneyBank);
            _walletStorage = new WalletStorage(_progress.Data.MoneyWallet);
            
            _pipe.Initialize(_bankStorage, _walletStorage);
        }
    }
}