using System;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.SaveLoad;
using _Project.Scripts.LocalizationSystem;
using _Project.Scripts.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstrappers
{
    public class SystemBootstrap : MonoBehaviour
    {
        [SerializeField] private BattleBootstrap _battleBootstrap;
        [SerializeField] private UIBootstrap _uiBootstrap;
        [SerializeField] private GameObject _loadingFader;
        
        private const int AwaitSeconds = 3;
        private ISaveLoadService _saveLoadService;
        
        [Inject] private PersistentProgress _progress;
        
        private void Awake()
        {
            _loadingFader.SetActive(true);
            InitializeServices();
        }

        private void Start()
        {
            CreateOrLoadData(
                (data) =>
                {
                    OnDataLoaded(data);
                    InitializeInstallers();
                });
        }

        private void InitializeServices()
        {
            var localizationLoader = new LocalizationLoader();
            Localization localizationService = new Localization(localizationLoader);
            LanguageDetector languageDetector = new LanguageDetector(localizationService);
            languageDetector.DetectSystemLanguage();
            
            localizationLoader.Load();
        }

        private void CreateOrLoadData(Action<PlayerData> onComplete)
        {
            ISaveLoadService cacheSaveLoad = new CacheSaveLoad();
            _saveLoadService = new CloudSaveLoad(cacheSaveLoad);
            
            if (_saveLoadService.HasKey(Constants.PlayerDataKey))
                _saveLoadService.Load<PlayerData>(Constants.PlayerDataKey, onComplete);
            else
                onComplete?.Invoke(new PlayerData());
        }

        private void OnDataLoaded(PlayerData data)
        {
            _progress.Data = data;
            Debug.Log("Data: " + JsonUtility.ToJson(data));
            _loadingFader.SetActive(false);

            TimerProgressSave().Forget();
        }

        private void InitializeInstallers()
        {
            _uiBootstrap.Initialize();
            _battleBootstrap.Initialize().Forget();
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