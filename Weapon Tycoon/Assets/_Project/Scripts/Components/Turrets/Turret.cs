using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Components.Turrets
{
    [Serializable]
    public struct TurretStat
    {
        public int Damage;
        public float RPM;
        public long BuyPrice;
    }
    
    public class Turret : MonoBehaviour
    {
        [SerializeField] private float _sensorZoneRadius;
        [SerializeField] private TurretStat _stat;
        [SerializeField] private LayerMask _enemyLayer;

        [SerializeField] private Transform _gunPoint1;
        [SerializeField] private Transform _gunPoint2;

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _explosionPrefab;

        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Transform _turretHead;
        [SerializeField] private Transform _turretGun1;
        [SerializeField] private Transform _turretGun2;

        private Enemy _target;
        private Collider[] _colliders;
        private WaitForSeconds _awaiter;
        private bool _gunPointChanger;
        
        private const int MaxColliders = 10;
        
        private void Start()
        {
            _colliders = new Collider[MaxColliders];
            _awaiter = new WaitForSeconds(60f / _stat.RPM);

            StartCoroutine(FiringCoroutine());
        }

        private void Update()
        {
            if (!_target) return;
            
            var targetRotation = Quaternion.LookRotation(_target.transform.position - _turretHead.position);
            _turretHead.rotation = Quaternion.RotateTowards(_turretHead.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private bool TryFindTarget(out Enemy enemy)
        {
            Physics.OverlapSphereNonAlloc(transform.position, _sensorZoneRadius, _colliders, _enemyLayer.value);

            enemy = null;
            float nearestDistance = Int32.MaxValue;
            for (int i = 0; i < _colliders.Length; i++)
            {
                if (!_colliders[i]) continue;
                if (_colliders[i].TryGetComponent(out Enemy spottedEnemy))
                {
                    var distance = Vector3.Distance(transform.position, spottedEnemy.transform.position);
                    if (distance >= nearestDistance) continue;
                    
                    nearestDistance = distance;
                    enemy = spottedEnemy;
                }
            }

            return enemy != null;
        }

        private void Shoot(Vector3 position)
        {
            var animatableGun = _gunPointChanger ? _turretGun1 : _turretGun2;
            var prevPosition = animatableGun.position;
            Sequence.Create()
                .Chain(Tween.Position(animatableGun, animatableGun.position - animatableGun.up * 0.2f, 0.05f))
                .Chain(Tween.Position(animatableGun, prevPosition, 0.05f));

            
            var spawnPosition = _gunPointChanger ? _gunPoint1.position : _gunPoint2.position;
            var bullet = GameObject.Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);

            //var targetedEnemy = _target;
            Tween.Position(bullet.transform, position, 0.15f)
                .OnComplete(() =>
                {
                    _target?.TakeDamage();
                    GameObject.Instantiate(_explosionPrefab, position, Quaternion.identity);
                    GameObject.Destroy(bullet);
                });

            _gunPointChanger = !_gunPointChanger;
        }
        
        IEnumerator FiringCoroutine()
        {
            while (true)
            {
                yield return _awaiter;
                do
                {
                    yield return null;
                    yield return null;
                    yield return null;
                    
                } while (TryFindTarget(out _target) == false);

                var position = _target.transform.position + Vector3.up * 1.5f;
                //_turretHead.LookAt(position);
                
                Shoot(position);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _sensorZoneRadius);

            if (_colliders == null)
                return;
            if (TryFindTarget(out var enemy))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(enemy.transform.position, 3f);
            }
        }
        #endif
    }
}