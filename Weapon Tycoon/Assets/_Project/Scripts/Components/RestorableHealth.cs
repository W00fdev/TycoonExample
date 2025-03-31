using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class RestorableHealth : Health
    {
        [SerializeField] private int _regeneration;
        private int _maxHp;

        public event Action<int, int> ChangedHealthEvent; 
        
        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);
        
        public override void Initialize(int hp)
        {
            base.Initialize(hp);
            _maxHp = hp;
            ChangedHealthEvent?.Invoke(_health, _maxHp);

            StartCoroutine(Regeneration());
        }

        public void UpgradeMaxHealth(int maxHealth)
        {
            _maxHp = maxHealth;
            ChangedHealthEvent?.Invoke(_health, _maxHp);
        }

        public void UpgradeRegeneration(int regeneration) => _regeneration = regeneration;

        public void Repair()
        {
            _health = _maxHp;
            ChangedHealthEvent?.Invoke(_health, _maxHp);
        }

        IEnumerator Regeneration()
        {
            while (true)
            {
                if (!IsAlive) continue;
                
                yield return _waitForSeconds;
                _health = Mathf.Clamp(_health + _regeneration, 0, _maxHp);
                
                ChangedHealthEvent?.Invoke(_health, _maxHp);
            }
        }
    }
}