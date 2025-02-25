using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.SaveLoad
{
    public interface ISaveLoadService
    {
        void Save<T>(string key, T data);
        void Save(string key, object data);
        void Load<T>(string key, Action<T> onComplete);
        bool HasKey(string key);
    }
}