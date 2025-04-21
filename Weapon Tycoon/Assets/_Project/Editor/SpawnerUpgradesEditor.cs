using _Project.Scripts.Infrastructure.Data.Spawners;
using UnityEditor;

namespace _Project.Editor
{
    [CustomEditor(typeof(SpawnerUpgradeConfig))]
    public class SpawnerUpgradesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.indentLevel += 2;
            
            // Change to editorgui
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 25f;
            EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Product Price", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Buy Price", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }
    }
}