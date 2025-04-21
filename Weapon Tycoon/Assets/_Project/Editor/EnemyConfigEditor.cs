using _Project.Scripts.Infrastructure.Data.Enemies;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    [CustomEditor(typeof(EnemyConfig))]
    public class EnemyConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var enemyConfig = (EnemyConfig)target;
            EditorGUILayout.LabelField(target.name.ToUpper(), EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            DrawDifficultyProgressBar(enemyConfig);
        }
        
        private void DrawDifficultyProgressBar(EnemyConfig config)
        {
            float damage = config.Data.Damage / EnemyConfig.MaxDamage;
            float cooldown = EnemyConfig.MinCooldown / config.Data.AtkCooldown;
            float speed = config.Data.Speed / EnemyConfig.MaxSpeed;
            float health = config.Data.Health / EnemyConfig.MaxHealth;

            GuardCondition(damage, nameof(damage));
            GuardCondition(cooldown, nameof(cooldown));
            GuardCondition(speed, nameof(speed));
            GuardCondition(health, nameof(health));
            
            float difficulty = Mathf.Clamp01((damage + cooldown + speed + health) / 4);

            Rect rect = GUILayoutUtility.GetRect(9, 18, "TextField");
            EditorGUI.ProgressBar(rect, difficulty, "Difficulty");
        }

        private void GuardCondition(float value, string propertyName)
        {
            if (value is > 1 or < 0)
                EditorGUILayout.HelpBox($"Caution: {propertyName} is over or less min/max values", MessageType.Warning);
        }


        private void GuardCondition()
        {
            
        }
    }
}
