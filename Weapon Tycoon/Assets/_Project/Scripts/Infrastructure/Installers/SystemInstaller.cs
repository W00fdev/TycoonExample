using System.Collections;
using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.Infrastructure.SaveLoad;
using _Project.Scripts.Utils;
using Playgama.Modules.Advertisement;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class SystemInstaller : MonoBehaviour
    {
        [SerializeField] private BattleInstaller _battleInstaller;
        [SerializeField] private UIInstaller _uiInstaller;
        
        [SerializeField] private GameObject _loadingFader;
        
        private readonly WaitForSeconds _timerProgressSave = new WaitForSeconds(3);
        private ISaveLoadService _saveLoadService;
        
        private void Start()
        {
            _loadingFader.SetActive(true);

            CreateOrLoadData();
        }

        private void CreateOrLoadData()
        {
            ISaveLoadService cacheSaveLoad = new CacheSaveLoad();
            _saveLoadService = new CloudSaveLoad(cacheSaveLoad);
            
            if (_saveLoadService.HasKey(Constants.PlayerDataKey))
                _saveLoadService.Load<PlayerData>(Constants.PlayerDataKey, OnDataLoaded);
            else
                OnDataLoaded(new PlayerData());
        }

        private void OnDataLoaded(PlayerData data)
        {
            var storage  = new StorageService();
            PersistentProgress.Instance = data;
            Debug.Log("Data: " + JsonUtility.ToJson(data));
            
            _uiInstaller.Initialize();
            _battleInstaller.Initialize(storage);

            StartCoroutine(TimerProgressSave());
            
            
            _loadingFader.SetActive(false);
        }

        IEnumerator TimerProgressSave()
        {
            while (true)
            {
                yield return _timerProgressSave;
                _saveLoadService.Save(Constants.PlayerDataKey, PersistentProgress.Instance);
                
                Debug.Log("Was saved: " + JsonUtility.ToJson(PersistentProgress.Instance));
            }
        }
    }
}