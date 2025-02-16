using System;
using System.Collections.Generic;
using System.Data;
using Playgama;
using Playgama.Modules.Storage;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.SaveLoad
{
    public class CloudSaveLoad : ISaveLoadService
    {
        private readonly ISaveLoadService _fallbackService;
        private readonly bool _cloudAvailable;
        
        public CloudSaveLoad(ISaveLoadService fallbackService)
        {
            _fallbackService = fallbackService;
            
            _cloudAvailable = Bridge.storage.IsSupported(StorageType.PlatformInternal) 
                && Bridge.storage.IsAvailable(StorageType.PlatformInternal);
            
            Debug.Log("Cloud available: " + _cloudAvailable);
        }
        
        public void Save<T>(string key, T data)
        {
            if (_cloudAvailable == false)
            {
                _fallbackService.Save(key, data);
                return;
            }

            var json = JsonUtility.ToJson(data);
            Bridge.storage.Set(key, json);
        }
        
        public void Save(string key, object data)
        {
            if (_cloudAvailable == false)
            {
                _fallbackService.Save(key, data);
                return;
            }
            
            var json = JsonUtility.ToJson(data);
            Bridge.storage.Set(key, json);
        }

        // Do not call Bridge.storage.Load in Awake!!!
        public void Load<T>(string key, Action<T> onLoaded)
        {
            if (_cloudAvailable == false)
            {
                _fallbackService.Load<T>(key, onLoaded);
                return;
            }

            Bridge.storage.Get(key, (success, json) =>
            {
                if (!success)
                    throw new ArgumentException($"not found key: {key}");
                
                if (string.IsNullOrEmpty(json))
                    throw new DataException($"no data at key: {key}");
                
                var data = JsonUtility.FromJson<T>(json);
                onLoaded?.Invoke(data);
            });
        }

        public bool HasKey(string key)
        {
            if (_cloudAvailable == false)
                return _fallbackService.HasKey(key);
            
            throw new NotImplementedException();
        }
    }
}