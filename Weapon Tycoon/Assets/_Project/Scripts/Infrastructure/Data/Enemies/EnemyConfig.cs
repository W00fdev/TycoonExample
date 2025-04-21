using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Data.Enemies
{
    [CreateAssetMenu(fileName = "Enemy Config Data", menuName = "Config/Enemies")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EnemyData _data;
        public EnemyData Data => _data;
        
        [Serializable]
        public class EnemyData
        {
            public float Speed;
            public float AtkCooldown;
            public int Damage;
            public int Health;
            public long Reward;
        }

        public const float MaxSpeed = 7;
        public const float MinCooldown = 0.5f;
        public const float MaxDamage = 100;
        public const float MaxHealth = 10000;
    }
}