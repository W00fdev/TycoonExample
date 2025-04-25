using System;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    public class JsonArrayEditor : EditorWindow
    {
        private Array array;
        private Action<Array> onSave;

        public void Initialize(Array array, Action<Array> onSave)
        {
            this.array = (Array)array.Clone();
            this.onSave = onSave;
        }

        private void OnGUI()
        {
            GUILayout.Label("Edit Array", EditorStyles.boldLabel);

            for (int i = 0; i < array.Length; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"[{i}]", GUILayout.Width(50));
                if (array.GetValue(i) is string strValue)
                {
                    array.SetValue(EditorGUILayout.TextField(strValue), i);
                }
                else if (array.GetValue(i) is int intValue)
                {
                    array.SetValue(EditorGUILayout.IntField(intValue), i);
                }
                else if (array.GetValue(i) is float floatValue)
                {
                    array.SetValue(EditorGUILayout.FloatField(floatValue), i);
                }
                else if (array.GetValue(i) is bool boolValue)
                {
                    array.SetValue(EditorGUILayout.Toggle(boolValue), i);
                }
                else
                {
                    GUILayout.Label(array.GetValue(i)?.ToString() ?? "null");
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Save"))
            {
                onSave?.Invoke(array);
                Close();
            }
        }
    }
}