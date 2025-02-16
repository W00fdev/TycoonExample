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
        
        private void Start()
        {
            _loadingFader.SetActive(true);

            CreateOrLoadData();
        }

        private void CreateOrLoadData()
        {
            ISaveLoadService cacheSaveLoad = new CacheSaveLoad();
            var cloudSaveLoad = new CloudSaveLoad(cacheSaveLoad);
            
            if (cloudSaveLoad.HasKey(Constants.PlayerDataKey))
                cloudSaveLoad.Load<PlayerData>(Constants.PlayerDataKey, OnDataLoaded);
            else
                OnDataLoaded(new PlayerData());
        }

        private void OnDataLoaded(PlayerData data)
        {
            var storage  = new StorageService();
            _battleInstaller.Initialize(storage);
            _uiInstaller.Initialize();

            PersistentProgress.Instance = data;
            
            Debug.Log("Data: " + JsonUtility.ToJson(data));
            
            _loadingFader.SetActive(false);
        }
    }
}