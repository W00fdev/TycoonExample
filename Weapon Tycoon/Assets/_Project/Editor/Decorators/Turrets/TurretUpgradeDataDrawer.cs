using _Project.Scripts.Infrastructure.Data.Turrets;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor.Decorators.Turrets
{
    [CustomPropertyDrawer(typeof(TurretUpgradeConfig.TurretStat))]
    public class TurretUpgradeDataDrawer : UnityEditor.PropertyDrawer
    {
        private SerializedProperty _damage;
        private SerializedProperty _rpm;
        private SerializedProperty _buyPrice;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            _damage = property.FindPropertyRelative("Damage");
            _rpm = property.FindPropertyRelative("RPM");
            _buyPrice = property.FindPropertyRelative("BuyPrice");

            EditorGUILayout.BeginHorizontal();
            DrawProperty(position, _damage, string.Empty, 0);
            DrawProperty(position, _rpm, string.Empty, 1);
            DrawProperty(position, _buyPrice, string.Empty, 2);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.EndProperty();
        }

        private void DrawProperty(Rect position, SerializedProperty property, string propertyName, int xIndex)
        {
            EditorGUIUtility.labelWidth = 40f;
            float xOffset = xIndex * position.size.x * 0.35f + 0.1f;
            
            float x = position.min.x + xOffset;
            float y = position.min.y + EditorGUIUtility.singleLineHeight;
            float width = position.size.x * 0.3f;
            float height = EditorGUIUtility.singleLineHeight;

            Rect drawArea = new Rect(x, y, width, height);
            EditorGUI.PropertyField(drawArea, property, new GUIContent(propertyName));
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            int totalLines = 2;
            return lineHeight * totalLines;
        }
    }
}