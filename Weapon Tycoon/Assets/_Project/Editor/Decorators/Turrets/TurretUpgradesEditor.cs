using _Project.Scripts.Infrastructure.Data.Turrets;
using UnityEditor;

namespace _Project.Editor.Decorators.Turrets
{
    [CustomEditor(typeof(TurretUpgradeConfig))]
    public class TurretUpgradesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.indentLevel += 2;
            
            /*
             *             public int Damage;
            public int RPM;
            public long BuyPrice;
             */
            
            // Change to editorgui
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 25f;
            EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("RPM", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Buy Price", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }
    }
}