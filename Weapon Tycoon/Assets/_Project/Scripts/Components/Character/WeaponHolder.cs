using System;
using System.Collections;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Factories.Accessors;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Components.Character
{
    [Serializable]
    public class WeaponConfig
    {
        public int Ammo;
        public int MaxAmmo;
        public int Damage;
        public float RPS;
        public float Spread;
    }
    
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _shootable;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        [SerializeField, ReadOnly] private BlasterType _type;

        private RaycastHit[] _hits;
        private ProjectileFactory _projectileFactory;
        private ExplosionFactory _explosionFactory;
        private int _damage;
        private float _cooldown;
        private float _timer;
        private bool _initialized;
        private bool _allowedFire;
        
        [Inject] private ProjectileFactoryAccessor<ProjectileFactory> _projectileFactoryAccessor;
        [Inject] private ProjectileFactoryAccessor<ExplosionFactory> _explosionFactoryAccessor;

        private IEnumerator Start()
        {
            while (_projectileFactoryAccessor.Factory == null || _explosionFactoryAccessor.Factory == null)
                yield return null;

            _hits = new RaycastHit[1];
            _projectileFactory = _projectileFactoryAccessor.Factory;
            _explosionFactory = _explosionFactoryAccessor.Factory;
            _initialized = true;
            _allowedFire = true;

            UpdateFromConfig();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UpdateFromConfig()
        {
            _damage = _weaponConfig.Damage;
            _cooldown = 1f / _weaponConfig.RPS;
        }

        private void Update()
        {
            if (!_initialized)
                return;


            if (_allowedFire && Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
                if (Physics.RaycastNonAlloc(ray, _hits, 1000f, _shootable.value) <= 0) 
                    return;
                
                Fire().Forget();
            }
            else
            {
                _timer += Time.deltaTime;
                if (_timer >= _cooldown)
                {
                    _timer -= _cooldown;
                    _allowedFire = true;
                }
            }

        }

        private async UniTaskVoid Fire()
        {
            var bullet = _projectileFactory.Next();
            bullet.transform.position = _gunPoint.position;
                
            // add distance-time scaling 

            var hit = _hits[0];
            await Tween.Position(bullet.transform, hit.point, 0.15f).ToYieldInstruction()
                .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());

            if (hit.collider.CompareTag("Enemy"))
            {
                if (TryGetComponent(out Health health))
                {
                    health.TakeDamage(_damage);
                }
            }
            
            MakeExplosion(_hits[0].point);

            bullet.ReturnToPool();
            //target?.TakeDamage(_data.Damage);

            _allowedFire = false;
        }

        private void MakeExplosion(Vector3 position)
        {
            var explosion = _explosionFactory.Next();
            explosion.transform.position = position;
            _source.PlayOneShot(_clip);
        }
    }
}