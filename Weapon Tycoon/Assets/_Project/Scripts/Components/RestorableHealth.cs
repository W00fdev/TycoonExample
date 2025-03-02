using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class RestorableHealth : Health
    {
        [SerializeField] private int _regeneration;
        private int _maxHp;

        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);
        
        public override void Initialize(int hp)
        {
            base.Initialize(hp);
            _maxHp = hp;

            StartCoroutine(Regeneration());
        }

        public void Repair()
            => _health = _maxHp;

        IEnumerator Regeneration()
        {
            while (true)
            {
                if (!IsAlive) continue;
                
                yield return _waitForSeconds;
                _health = Mathf.Clamp(_health + _regeneration, 0, _maxHp);
            }
        }
    }
}