﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using _Project.Scripts.UI.Models;
using _Project.Scripts.UI.Views;
using _Project.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public abstract class BlasterSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerInfoView _infoView;
        
        [SerializeField] private Vector3 _defaultWeaponRotation;
        [SerializeField] private Vector3 _defaultBoxRotation;

        #region SpawnPoints
        [SerializeField] protected Transform _boxSpawnPoint;
        [SerializeField] protected Transform _boxEndPoint;
        [SerializeField] protected Transform _blasterSpawnPoint;
        [SerializeField] protected Transform _blasterEndPoint;
        [SerializeField] protected Transform _moneyTextSpawnPoint;
        #endregion
        
        [SerializeField] protected ParticleSystem _particles;

        [Header("Debug only")]
        [SerializeField] private int _movablesCapacity;
        [ShowInInspector, ReadOnly] private List<PooledView> _movableBoxes;
        [ShowInInspector, ReadOnly] private List<PooledView> _movableBlasters;
        
        private SpawnerData _spawnerData;
        private BlasterFactory _blasterFactory;
        private BoxFactory _boxFactory;
        private MoneyTextFactory _moneyTextFactory;

        [SerializeField, ReadOnly] private float _speedInPercents;
        private WaitForSeconds _waiter;

        public void Initialize(BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, SpawnerData spawnerData)
        {
            _boxFactory = boxFactory;
            _moneyTextFactory = moneyTextFactory;
            _spawnerData = spawnerData;
            _speedInPercents = _spawnerData.SpawnerSpeed / 3f;
            
            _movableBoxes = new (_movablesCapacity);
            _movableBlasters = new (_movablesCapacity);
            
            //spawnerData.Initialize();
            _infoView.Initialize(spawnerData.SpawnerName);
            _infoView.UpdateInfo(spawnerData.SpawnerSpeed.ToSpeedFormat(), spawnerData.ProductPrice.ToString());
            
            _spawnerData.SpawnerDataChanged += UpgradeSpawner;
        }

        private void OnDestroy()
        {
            _spawnerData.SpawnerDataChanged -= UpgradeSpawner;
        }

        private void UpgradeSpawner()
        {
            _waiter = new WaitForSeconds(1f / _spawnerData.SpawnerSpeed);
            _speedInPercents = _spawnerData.SpawnerSpeed / 3f;
            
            _infoView.UpdateInfo(_spawnerData.SpawnerSpeed.ToSpeedFormat(), _spawnerData.ProductPrice.ToString());
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
            weapon.ViewReturner -= ConsumeWeapon;
            _movableBlasters.Remove(weapon);

            EventBus.BankIncome.Invoke(_spawnerData.ProductPrice);
            SpawnText();
        }

        private void SpawnText()
        {
            var moneyText = _moneyTextFactory.Next();
            moneyText.transform.position = _moneyTextSpawnPoint.position;
            ((MoneyTextView)moneyText).SetText($"+{_spawnerData.ProductPrice.ToHeaderMoneyFormat()}");
            ((MoneyTextView)moneyText).PlayTextAnimation();
        }
        
        IEnumerator SpawnTimer()
        {
            _waiter = new WaitForSeconds(1f / _spawnerData.SpawnerSpeed);
            
            while (true)
            {
                yield return _waiter;
                SpawnBox();
            }
        }
    }
}