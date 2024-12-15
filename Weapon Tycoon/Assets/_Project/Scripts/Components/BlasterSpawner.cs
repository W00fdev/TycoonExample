using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using _Project.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public abstract class BlasterSpawner : MonoBehaviour
    {
        [SerializeField] private Vector3 _defaultWeaponRotation;
        [SerializeField] private Vector3 _defaultBoxRotation;
        
        [SerializeField] protected Transform _boxSpawnPoint;
        [SerializeField] protected Transform _boxEndPoint;
        [SerializeField] protected Transform _blasterSpawnPoint;
        [SerializeField] protected Transform _blasterEndPoint;
        [SerializeField] protected Transform _moneyTextSpawnPoint;
        
        [SerializeField] protected ParticleSystem _particles;

        [SerializeField] private float _speedInPercents;
        [SerializeField] private int _movablesCapacity;
        
        [ShowInInspector] private List<PooledView> _movableBoxes;
        [ShowInInspector] private List<PooledView> _movableBlasters;
        
        private WeaponSpawnerData _spawnerData;
        private BlasterFactory _blasterFactory;
        private BoxFactory _boxFactory;
        private MoneyTextFactory _moneyTextFactory;

        public void Initialize(BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, WeaponSpawnerData spawnerData)
        {
            _boxFactory = boxFactory;
            _moneyTextFactory = moneyTextFactory;
            _spawnerData = spawnerData;

            _movableBoxes = new (_movablesCapacity);
            _movableBlasters = new (_movablesCapacity);
        }

        public virtual void Resolve(BlasterFactory blasterFactory)
        {
            _blasterFactory = blasterFactory;
            StartCoroutine(SpawnTimer());
        }

        private void Update()
        {
            MoveViews(_movableBoxes, _boxSpawnPoint, _boxEndPoint);
            MoveViews(_movableBlasters, _blasterSpawnPoint, _blasterEndPoint);
        }

        private void MoveViews(List<PooledView> movables, Transform fromPoint, Transform toPoint)
        {
            if (movables.Count <= 0) 
                return;
            
            for (int i = 0; i < movables.Count; i++) 
                MoveView(movables[i].transform, fromPoint.position, toPoint.position);

            PooledView firstMovable = movables[0];
            ReturnViewIfNeeded(firstMovable, fromPoint.position, toPoint.position);
        }

        private void MoveView(Transform view, Vector3 from, Vector3 to)
        {
            float viewInversedLerp = view.position.InverseLerp(from, to);
            viewInversedLerp += _speedInPercents * Time.deltaTime;

            view.position = Vector3.Lerp(from, to, viewInversedLerp);
        }

        private void ReturnViewIfNeeded(PooledView view, Vector3 from, Vector3 to)
        {
            float viewInversedLerp = view.transform.position.InverseLerp(from, to);
            
            if (viewInversedLerp >= 1f)
                view.ReturnToPool();
        }

        private void SpawnBox()
        {
            var box = _boxFactory.Next();
            box.transform.SetPositionAndRotation(_boxSpawnPoint.position, Quaternion.Euler(_defaultBoxRotation));
            _movableBoxes.Add(box);

            box.ViewReturner += SpawnWeapon;
        }

        private void SpawnWeapon(PooledView box)
        {
            _movableBoxes.Remove(box);
            box.ViewReturner -= SpawnWeapon;
            
            _particles.Play();
            
            var blaster = _blasterFactory.Next();
            blaster.transform.SetPositionAndRotation(_blasterSpawnPoint.position, Quaternion.Euler(_defaultWeaponRotation));
            blaster.ViewReturner += ConsumeWeapon;
            
            _movableBlasters.Add(blaster);
        }

        private void ConsumeWeapon(PooledView weapon)
        {
            _movableBlasters.Remove(weapon);
            SpawnText((WeaponView)weapon);
        }

        public void SpawnText(IEntity entity)
        {
            var moneyText = _moneyTextFactory.Next();
            moneyText.transform.position = _moneyTextSpawnPoint.position;
            ((MoneyTextView)moneyText).SetText($"+{entity.Entity.Price}$");
            ((MoneyTextView)moneyText).PlayTextAnimation();
        }
        
        IEnumerator SpawnTimer()
        {
            var waiter = new WaitForSeconds(1f / _spawnerData.Speed);
            
            while (true)
            {
                yield return waiter;
                SpawnBox();
            }
        }
    }
}