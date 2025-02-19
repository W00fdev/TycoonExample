using System;
using _Project.Scripts.Infrastructure.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Infrastructure.SaveLoad
{
    public class CacheSaveLoad : ISaveLoadService
    {
        public void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
        }
        
        public void Save(string key, object data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
        }

        public void Load<T>(string key, Action<T> onLoaded)
        {
            if (HasKey(key) == false)
                throw new ArgumentException($"not found key: {key}");

            var json = PlayerPrefs.GetString(key);
            var data = JsonUtility.FromJson<T>(json);

            onLoaded?.Invoke(data);
        }

        public bool HasKey(string key)
            => PlayerPrefs.HasKey(key);
    }
}