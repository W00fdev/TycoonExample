using System;
using System.Collections;
using _Project.Scripts.Data;
using _Project.Scripts.Infrastructure.Factories;
using _Project.Scripts.Infrastructure.Factories.Accessors;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

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
        [Header("Impact animations")]
        [SerializeField] private Transform _ikTarget;
        [SerializeField] private Transform _armConstraintTarget;
        [SerializeField] private Transform _armDefaultPivot;
        [SerializeField] private MultiAimConstraint _headConstraint;
        [SerializeField] private TwoBoneIKConstraint _armConstraint;
        [SerializeField] private Animator _animator;
        
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _shootable;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        [SerializeField, ReadOnly] private BlasterType _type;

        private DefaultProjectileFactory _projectileFactory;
        private ExplosionFactory _explosionFactory;
        private int _damage;
        private float _cooldown;
        private float _spread;
        private float _timer;
        private bool _initialized;
        private bool _allowedFire;
        private bool _constraintAnimating;

        private readonly Vector3 _strength = new Vector3(0.05f, 0.1f, 0.5f);
        
        [Inject] private ProjectileFactoryAccessor<DefaultProjectileFactory> _projectileFactoryAccessor;
        [Inject] private ProjectileFactoryAccessor<ExplosionFactory> _explosionFactoryAccessor;

        private IEnumerator Start()
        {
            while (_projectileFactoryAccessor.Factory == null || _explosionFactoryAccessor.Factory == null)
                yield return null;

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
            _spread = _weaponConfig.Spread;
        }

        private void Update()
        {
            if (!_initialized)
                return;
            
            Ray ray = _mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
            
            bool hasHit = false;
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _shootable.value))
            {
                _ikTarget.position = hit.point;
             
                if (_constraintAnimating == false)
                    _armConstraintTarget.position = Vector3.Slerp(_armConstraintTarget.position, hit.point, 0.15f);
                
                _headConstraint.weight = Mathf.Lerp(_headConstraint.weight, 
                    Vector3.Angle(transform.right, _ikTarget.position - transform.position) > 90 
                        ? 0f 
                        : 1f,
                    0.1f);
                
                hasHit = true;
            }

            var hasInput = Input.GetMouseButton(0);
            _armConstraint.weight = Mathf.Lerp(_armConstraint.weight, hasInput 
                ? 1f 
                : 0f, 0.1f);

            if (_allowedFire && hasInput && hasHit)
            {
                Ray spreadedRay = _mainCamera.ViewportPointToRay(Vector3.one * 0.5f 
                                                                 + new Vector3(Random.value * _spread, Random.value * _spread));

                if (Physics.Raycast(spreadedRay, out RaycastHit spreadedHit, 1000f, _shootable.value) == false)
                    return;
                
                Fire(spreadedHit).Forget();
                _allowedFire = false;
                _timer -= _cooldown;

                _constraintAnimating = true;
                _armConstraintTarget.position = _armDefaultPivot.position;

                Tween.PunchLocalPosition(_armConstraintTarget,_strength, 0.1f)
                    .OnComplete(() => _constraintAnimating = false);
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

        private async UniTaskVoid Fire(RaycastHit hitInfo)
        {
            var bullet = _projectileFactory.Next();
            bullet.transform.position = _gunPoint.position;
                
            // add distance-time scaling 

            Health target = null;
            if (hitInfo.collider.CompareTag("Enemy"))
                hitInfo.collider.TryGetComponent(out target);
            
            await Tween.Position(bullet.transform, hitInfo.point, 0.15f).ToYieldInstruction()
                .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            
            MakeExplosion(hitInfo.point);

            target?.TakeDamage(_damage);
            bullet.ReturnToPool();
        }

        private void MakeExplosion(Vector3 position)
        {
            var explosion = _explosionFactory.Next();
            explosion.transform.position = position;
            _source.PlayOneShot(_clip);
        }
    }
}