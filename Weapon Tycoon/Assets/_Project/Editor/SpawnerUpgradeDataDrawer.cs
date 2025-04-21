using System;
using _Project.Scripts.Infrastructure.Data.Spawners;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    [CustomPropertyDrawer(typeof(SpawnerUpgradeConfig.UpgradeData))]
    public sealed class SpawnerUpgradeDataDrawer : UnityEditor.PropertyDrawer
    {
        private SerializedProperty _speed;
        private SerializedProperty _productPrice;
        private SerializedProperty _buyPrice;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            //base.OnGUI(position, property, label);

            _speed = property.FindPropertyRelative("Speed");
            _productPrice = property.FindPropertyRelative("ProductPrice");
            _buyPrice = property.FindPropertyRelative("BuyPrice");
            
            /*Rect foldOutBox = new Rect(position.min.x, position.min.y,
                position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);*/

            //if (property.isExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                DrawProperty(position, _speed, string.Empty, 0);
                DrawProperty(position, _productPrice, string.Empty, 1);
                DrawProperty(position, _buyPrice, string.Empty, 2);
                EditorGUILayout.EndHorizontal();
            }
            
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
