using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public sealed class RestorableHealth : IDamageable, IRestorable
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly HealthComponent _health;
        private int _regeneration;
        private int _maxHp;

        private Coroutine _regenerationRoutine;

        public bool IsAlive => _health.IsAlive;
        
        public event Action<int, int> ChangedHealthEvent;

        private static readonly WaitForSeconds RegenerationCooldown = new WaitForSeconds(1f);

        public RestorableHealth(ICoroutineRunner coroutineRunner, HealthComponent health, 
            int maxHp, int regeneration)
        {
            _coroutineRunner = coroutineRunner;
            _health = health;
            _maxHp = maxHp;
            _regeneration = regeneration;

            _regenerationRoutine = _coroutineRunner.StartCoroutine(Regeneration());
        }

        public void TakeDamage(int damage)
        {
            if (IsAlive == false)
                return;
            
            _health.TakeDamage(damage);
            OnChangedHealth();
        }

        public void Restore(int amount)
        {
            if (_health.Health >= _maxHp)
                return;

            _health.Restore(Mathf.Clamp(amount, 0, _maxHp - _health.Health));
            OnChangedHealth();
        }

        public void UpgradeMaxHealth(int maxHealth, bool notify = true)
        {
            _maxHp = maxHealth;
            if (notify)
                OnChangedHealth();
        }

        public void UpgradeRegeneration(int regeneration)
        {
            _regeneration = regeneration;
        }

        public void Repair()
        {
            Restore(_maxHp);
            _regenerationRoutine ??= _coroutineRunner.StartCoroutine(Regeneration());
        }

        private void OnChangedHealth() => ChangedHealthEvent?.Invoke(_health.Health, _maxHp);

        IEnumerator Regeneration()
        {
            while (true)
            {
                if (!IsAlive)
                {
                    _regenerationRoutine = null;
                    yield break;
                }

                yield return RegenerationCooldown;
                Restore(_regeneration);
            }
        }
    }
}