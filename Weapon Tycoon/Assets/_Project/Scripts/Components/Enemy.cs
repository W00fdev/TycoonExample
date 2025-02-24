using System;
using System.Collections;
using PrimeTween;
using Unity.Behavior;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform _basicModel;
        [SerializeField] private int _health;
        [SerializeField] private BehaviorGraphAgent _agent;
        [SerializeField] private Animator _animator;

        private const string DeathTrigger = "Death";
        private int _deathHash;
        private readonly WaitForSeconds _secondsToDie = new WaitForSeconds(2.7f);

        public bool IsAlive => _health > 0;
        
        private void Awake()
        {
            _deathHash = Animator.StringToHash(DeathTrigger);
        }

        public void TakeDamage()
        {
            Tween.PunchScale(_basicModel, Vector3.up * 0.1f, 0.1f);
            _health -= 1;

            if (IsAlive == false)
            {
                _animator.SetTrigger(_deathHash);
                _agent.Graph.End();
                StartCoroutine(DelayedCall());
            }
        }

        IEnumerator DelayedCall()
        {
            yield return _secondsToDie;
            GameObject.Destroy(gameObject);
        }
    }
}