using _Project.Scripts.CurrencyModule.Models;
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
            _bankStorage = new BankStorage(0);
            _walletStorage = new WalletStorage(0);
            
            _pipe.Initialize(_bankStorage, _walletStorage);
        }
    }
}