using System;
using System.Collections;
using _Project.Scripts.Infrastructure.Data.Turrets;
using _Project.Scripts.UI.Views.Turrets;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Components.Turrets
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretInfoView _infoView;
        [SerializeField] private string _turretNameKey;
        [SerializeField] private float _sensorZoneRadius;
        [SerializeField] private LayerMask _enemyLayer;

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _explosionPrefab;
        
        [Header("Impact visual")]
        [SerializeField] protected ParticleSystem _upgradeImpact;
        [SerializeField] protected float _endScaleYSquash = 2f;
        [SerializeField] protected GameObject[] _upgradesVisual;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Transform _turretHead;
        [SerializeField] private Transform _turretGun1;
        [SerializeField] private Transform _turretGun2;
        [SerializeField] private Transform _gunPoint1;
        [SerializeField] private Transform _gunPoint2;

        protected int _upgradeVisualLevel;
        
        private TurretData _data;
        private Enemy _target;
        private Collider[] _colliders;
        private WaitForSeconds _awaiter;
        private bool _gunPointChanger;
        
        private const int MaxColliders = 10;
        
        public virtual void Initialize(TurretData turretData)
        {
            _colliders = new Collider[MaxColliders];
            _data = turretData;
            
            _infoView.Initialize(_turretNameKey);
            _infoView.UpdateInfo(_data.RPM.ToString(), _data.Damage.ToString());
            
            _data.TurretDataChanged += UpgradeTurret;
        }
        
        public void Resolve()
        {
            FiringTimer().Forget();
        }

        private void OnDestroy()
        {
            _data.TurretDataChanged -= UpgradeTurret;
        }

        private void UpgradeTurret()
        {
            float yPrevScale = _turretHead.localScale.y;
            Sequence.Create(cycles: 1)
                .Chain(Tween.ScaleY(_turretHead, _endScaleYSquash, 0.25f, Ease.InBack))
                .Chain(Tween.ScaleY(_turretHead, yPrevScale, 0.25f, Ease.OutBack));
            
            _infoView.UpdateInfo(_data.RPM.ToString(), _data.Damage.ToString());
            
            _upgradeVisualLevel = Mathf.Min(_upgradeVisualLevel + 1, _upgradesVisual.Length);
            _upgradeImpact.Play();

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            for (int i = 0; i < Mathf.Min(_upgradeVisualLevel, _upgradesVisual.Length); i++)
                _upgradesVisual[i].SetActive(true);
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
                    if (spottedEnemy.IsAlive == false)
                        continue;
                    
                    var distance = Vector3.Distance(transform.position, spottedEnemy.transform.position);
                    if (distance >= nearestDistance) continue;
                    
                    nearestDistance = distance;
                    enemy = spottedEnemy;
                }
            }

            return enemy;
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
            Tween.Position(bullet.transform, position, 0.15f)
                .OnComplete(() =>
                {
                    _target?.TakeDamage();
                    GameObject.Instantiate(_explosionPrefab, position, Quaternion.identity);
                    GameObject.Destroy(bullet);
                });

            _gunPointChanger = !_gunPointChanger;
        }
        
        async UniTaskVoid FiringTimer()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(60f / _data.RPM),
                    cancellationToken: this.GetCancellationTokenOnDestroy());

                do
                {
                    await UniTask.DelayFrame(delayFrameCount: 3);
                } while (TryFindTarget(out _target) == false);

                var position = _target.transform.position + Vector3.up * 1.5f;
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