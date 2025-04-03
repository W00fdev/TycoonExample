using System;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Loading;
using _Project.Scripts.Infrastructure.SaveLoad;
using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.Infrastructure.States.GameState;
using _Project.Scripts.LogicModule;
using _Project.Scripts.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstrappers
{
    public class SystemBootstrap : MonoBehaviour, IInitializable
    {
        [SerializeField] private UIBootstrap _uiBootstrap;
        [SerializeField] private EconomyShop _economyShop;
        [SerializeField] private DefenseShop _defenseShop;

        private const int AwaitSeconds = 3;

        private LoadingCurtainService _curtainService;
        private ISaveLoadService _saveLoadService;
        private PersistentProgress _progress;
        private GameStateMachine _gameStateMachine;
        
        [Inject]
        private void Construct(LoadingCurtainService curtainService, ISaveLoadService saveLoadService,
            PersistentProgress progress, GameStateMachine gameStateMachine)
        {
            _curtainService = curtainService;
            _saveLoadService = saveLoadService;
            _progress = progress;
            _gameStateMachine = gameStateMachine;
            
            Debug.Log("System Bootstrap Constructed");
        }
        
        public void Initialize()
        {
            Debug.Log("System Bootstrap Initializable");

        }
        
        private void Start()
        {
            _curtainService.ShowInstant();
            Debug.Log("System Bootstrap Started");

            //_gameStateMachine.SwitchState<BootstrapState>();
            TimerProgressSave().Forget();
        }
        
        async UniTaskVoid TimerProgressSave()
        {
            try
            {
                while (true)
                {
                    _saveLoadService.Save(Constants.PlayerDataKey, _progress.Data);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(AwaitSeconds),
                        cancellationToken: this.GetCancellationTokenOnDestroy());
                }
            }
            catch (Exception e)
            {
                Debug.Log("You should check your internet connection, before can continue to play!");
                Debug.Log($"Log to crashlytics {e}");
                throw;
            }
        }
    }
}