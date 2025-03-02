using System;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class Health : MonoBehaviour
    {
        [SerializeField] protected int _health;

        public bool IsAlive => _health > 0;
        public event Action DamagedEvent;
        public event Action DiedEvent;
        
        public virtual void Initialize(int hp)
        {
            _health = hp;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            DamagedEvent?.Invoke();
            
            if (_health <= 0)
                DiedEvent?.Invoke();
        }
        
        
    }
}