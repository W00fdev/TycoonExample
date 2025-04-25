using _Project.Scripts.Infrastructure.Data.BigBeautifulWall;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor.Decorators.Wall
{
    [CustomPropertyDrawer(typeof(WallUpgradeConfig.UpgradeData))]
    public sealed class WallUpgradeDataDrawer : UnityEditor.PropertyDrawer
    {
        private SerializedProperty _regeneration;
        private SerializedProperty _health;
        private SerializedProperty _buyPrice;
        private SerializedProperty _repairPrice;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            _regeneration = property.FindPropertyRelative("Regeneration");
            _health = property.FindPropertyRelative("Health");
            _buyPrice = property.FindPropertyRelative("BuyPrice");
            _repairPrice = property.FindPropertyRelative("RepairPrice");

            EditorGUILayout.BeginHorizontal();
            DrawProperty(position, _regeneration, string.Empty, 0);
            DrawProperty(position, _health, string.Empty, 1);
            DrawProperty(position, _buyPrice, string.Empty, 2);
            DrawProperty(position, _repairPrice, string.Empty, 3);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.EndProperty();
        }

        private void DrawProperty(Rect position, SerializedProperty property, string propertyName, int xIndex)
        {
            EditorGUIUtility.labelWidth = 25f;
            float xOffset = xIndex * position.size.x * 0.25f + 0.15f;
            
            float x = position.min.x + xOffset;
            float y = position.min.y + EditorGUIUtility.singleLineHeight;
            float width = position.size.x * 0.2f;
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