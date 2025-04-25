using System;
using System.Linq;
using Microsoft.Win32;

namespace _Project.Editor.PreferencesViewer
{
    public abstract class PreferencesStorage
    {
        protected readonly string _prefPath;
        protected string[] _cachedData = Array.Empty<string>();

        public PreferencesStorage(string prefPath)
        {
            _prefPath = prefPath;
        }

        public abstract string[] GetKeys();
    }
    
    public sealed class WindowsPreferencesStorage : PreferencesStorage
    {
        public WindowsPreferencesStorage(string prefPath) : base(prefPath)
        {
        }

        public override string[] GetKeys()
        {
            _cachedData = Array.Empty<string>();

            using RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(_prefPath);
            if (rootKey == null) return _cachedData;
            
            _cachedData = rootKey.GetValueNames()
                .Where(key => !(key.StartsWith("unity.") || key.StartsWith("UnityGraphicsQuality")))
                .Select(key => key[..key.LastIndexOf("_h", StringComparison.Ordinal)])
                .ToArray();
                
            rootKey.Close();

            return _cachedData;
        }
    }

    public sealed class NullPreferencesStorage : PreferencesStorage
    {
        public NullPreferencesStorage(string prefPath) : base(prefPath)
        {
        }

        public override string[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}