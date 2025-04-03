using System;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.Loading;
using _Project.Scripts.Infrastructure.SaveLoad;
using _Project.Scripts.LocalizationSystem;
using _Project.Scripts.Utils;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.States.GameState
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)] 
    public class BootstrapState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly ISaveLoadService _saveLoadService;
        private readonly PersistentProgress _progress;
        private readonly LoadingCurtainService _loadingCurtain;
        
        public BootstrapState(GameStateMachine stateSwitcher, ISaveLoadService saveLoadService,
            PersistentProgress progress, LoadingCurtainService loadingCurtain)
        {
            _stateSwitcher = stateSwitcher;
            _saveLoadService = saveLoadService;
            _progress = progress;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            InitializeServices();
            CreateOrLoadData(OnProgressLoaded);

            PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        }

        public void Exit()
        {
        }
        
        private void CreateOrLoadData(Action<PlayerData> onComplete)
        {
            if (_saveLoadService.HasKey(Constants.PlayerDataKey))
                _saveLoadService.Load<PlayerData>(Constants.PlayerDataKey, onComplete);
            else
                onComplete?.Invoke(new PlayerData());
        }

        private void OnProgressLoaded(PlayerData data)
        {
            _progress.Data = data;
            Debug.Log("Data: " + JsonUtility.ToJson(data));
            
            _loadingCurtain.HideAsync().Forget();
            
            _stateSwitcher.SwitchState<AssetsLoadingState>();
        }
        
        private void InitializeServices()
        {
            var localizationLoader = new LocalizationLoader();
            Localization localizationService = new Localization(localizationLoader);
            LanguageDetector languageDetector = new LanguageDetector(localizationService);
            languageDetector.DetectSystemLanguage();
            
            localizationLoader.Load();
        }
    }
}