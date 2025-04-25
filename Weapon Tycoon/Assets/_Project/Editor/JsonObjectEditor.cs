using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    public class JsonObjectEditor : EditorWindow
    {
        private Dictionary<string, object> dict;
        private Action<Dictionary<string, object>> onSave;

        public void Initialize(Dictionary<string, object> dict, Action<Dictionary<string, object>> onSave)
        {
            this.dict = new Dictionary<string, object>(dict);
            this.onSave = onSave;
        }

        private void OnGUI()
        {
            GUILayout.Label("Edit Object", EditorStyles.boldLabel);

            foreach (var key in new List<string>(dict.Keys))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(key, GUILayout.Width(150));
                if (dict[key] is string strValue)
                {
                    dict[key] = EditorGUILayout.TextField(strValue);
                }
                else if (dict[key] is int intValue)
                {
                    dict[key] = EditorGUILayout.IntField(intValue);
                }
                else if (dict[key] is float floatValue)
                {
                    dict[key] = EditorGUILayout.FloatField(floatValue);
                }
                else if (dict[key] is bool boolValue)
                {
                    dict[key] = EditorGUILayout.Toggle(boolValue);
                }
                else
                {
                    GUILayout.Label(dict[key]?.ToString() ?? "null");
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Save"))
            {
                onSave?.Invoke(dict);
                Close();
            }
        }
    }
}