using _Project.Scripts.Infrastructure.Data.BigBeautifulWall;
using UnityEditor;

namespace _Project.Editor.Decorators.Wall
{
    [CustomEditor(typeof(WallUpgradeConfig))]
    public class WallUpgradesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.indentLevel += 2;
            
            // Change to editorgui
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 25f;
            EditorGUILayout.LabelField("Regeneration", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Buy Price", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Repair Price", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }
    }
}