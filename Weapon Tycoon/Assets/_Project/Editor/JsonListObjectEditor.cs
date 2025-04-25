using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    public class JsonListEditor<T> : EditorWindow
    {
        protected List<T> _list;
        protected Action<List<T>> _onSave;

        public void Initialize(List<T> list, Action<List<T>> onSave)
        {
            _list = new List<T>(list);
            _onSave = onSave;
        }

        protected virtual void OnGUI()
        {
            if (GUILayout.Button("Add Item"))
            {
                _list.Add(default);
            }

            if (GUILayout.Button("Save"))
            {
                _onSave?.Invoke(_list);
                Close();
            }
        }
    }

    public class JsonListObjectEditor : JsonListEditor<object>
    {
        protected override void OnGUI()
        {
            GUILayout.Label("Edit List", EditorStyles.boldLabel);

            for (int i = 0; i < _list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"[{i}]", GUILayout.Width(50));
                GUILayout.Label(_list[i].GetType().ToString(), GUILayout.Width(50));
                
                GUILayout.Label(_list[i]?.ToString() ?? "null");
                
                GUILayout.EndHorizontal();
            }

            base.OnGUI();
        }

    }
}