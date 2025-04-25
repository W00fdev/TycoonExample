using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    public class JsonListFloatEditor : JsonListEditor<float>
    {
        protected override void OnGUI()
        {
            GUILayout.Label("Edit List", EditorStyles.boldLabel);

            for (int i = 0; i < _list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"[{i}]", GUILayout.Width(50));
                GUILayout.Label(_list[i].GetType().ToString(), GUILayout.Width(50));
                
                _list[i] = DrawEditableField(_list[i]);
                
                GUILayout.EndHorizontal();
            }
            
            base.OnGUI();
        }

        private float DrawEditableField(float value) => EditorGUILayout.FloatField(value);
    }
}