using System;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor.PreferencesViewer
{
    public class PreferencesViewer : EditorWindow
    {
        private PreferencesStorage _preferencesStorage;
        private string[] _cachedPreferences = Array.Empty<string>();
        
        private string RegistryPath = string.Empty;
        private static readonly string PlatformPathPrefix = @"<CurrentUser>";
        
        // It works only for windows editor 
        [MenuItem("Tools/Preferences Viewer")]
        public static void ShowWindow()
        {
            var window = GetWindow<PreferencesViewer>(false, "Preferences Viewer");
            window.minSize = new Vector2(270f, 300f);
            
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(20);
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField(new GUIContent("Player Preferences:"));
            foreach (var key in _cachedPreferences)
            {
                if (GUILayout.Button(new GUIContent(key)))
                {
                    JsonEditor.ShowWindow(PlayerPrefs.GetString(key));
                }
            }
            
            EditorGUILayout.EndVertical();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR_WIN
            RegistryPath = @"SOFTWARE\Unity\UnityEditor\" 
                           + PlayerSettings.companyName 
                           + @"\" 
                           + PlayerSettings.productName;
            _preferencesStorage = new WindowsPreferencesStorage(RegistryPath);
#else
            _preferencesStorage = new NullPreferencesStorage(RegistryPath);
#endif

            _cachedPreferences = _preferencesStorage.GetKeys();
            foreach (var key in _cachedPreferences)
            {
                Debug.Log("Registry: " + key);
            }
        }

    }
}