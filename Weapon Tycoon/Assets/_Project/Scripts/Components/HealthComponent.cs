using System;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class HealthComponent : MonoBehaviour, IDamageable, IRestorable
    {
        [SerializeField] protected int _health;

        public int Health => _health;
        
        public bool IsAlive => _health > 0;
        public event Action DamagedEvent;
        public event Action DiedEvent;
        
        public void Initialize(int hp)
        {
            _health = hp;
        }

        public void TakeDamage(int damage)
        {
            _health = Mathf.Clamp(_health - damage, 0, _health);
            DamagedEvent?.Invoke();
            
            if (_health <= 0)
                DiedEvent?.Invoke();
        }

        public void Restore(int amount) 
            => _health = Mathf.Clamp(_health + amount, _health, int.MaxValue);
    }
}