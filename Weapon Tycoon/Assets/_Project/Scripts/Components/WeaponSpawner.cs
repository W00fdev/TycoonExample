using System.Collections;
using _Project.Scripts.Animations;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.LogicModule.Factories;
using _Project.Scripts.LogicModule.Views;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public abstract class WeaponSpawner : MonoBehaviour
    {
        [SerializeField] private Vector3 _defaultWeaponRotation;
        
        [SerializeField] protected Transform _boxSpawnPoint;
        [SerializeField] protected Transform _weaponSpawnPoint;
        [SerializeField] protected Transform _moneyTextSpawnPoint;
        
        [SerializeField] protected ParticleSystem _particles;
        
        private WeaponSpawnerData _spawnerData;
        private WeaponFactory _weaponFactory;
        private BoxFactory _boxFactory;
        private MoneyTextFactory _moneyTextFactory;

        public void Initialize(BoxFactory boxFactory, MoneyTextFactory moneyTextFactory, WeaponSpawnerData spawnerData)
        {
            _boxFactory = boxFactory;
            _moneyTextFactory = moneyTextFactory;
            _spawnerData = spawnerData;
        }

        public virtual void Resolve(WeaponFactory weaponFactory)
        {
            _weaponFactory = weaponFactory;
            StartCoroutine(SpawnTimer());
        }

        private void SpawnBox()
        {
            var box = _boxFactory.Next();
            box.transform.position = _boxSpawnPoint.position;
            box.transform.rotation = Quaternion.Euler(Vector3.zero);


            var conveyorMovable = box.GetComponent<ConveyorMovable>();
            conveyorMovable.SetSpeed(5f);
            conveyorMovable.ClearInertia();
            
            box.ViewReturner += SpawnWeapon;
        }

        private void SpawnWeapon(PooledView box)
        {
            box.ViewReturner -= SpawnWeapon;
            
            _particles.Play();
            
            var weapon = _weaponFactory.Next();
            weapon.transform.position = _weaponSpawnPoint.position;
            weapon.transform.rotation = Quaternion.Euler(_defaultWeaponRotation);
            
            weapon.GetComponent<ConveyorMovable>().SetSpeed(5f);
            weapon.GetComponent<ConveyorMovable>().ClearInertia();
            
            weapon.ViewReturner += ConsumeWeapon;
        }

        private void ConsumeWeapon(PooledView weapon)
        {
            weapon.ViewReturner -= ConsumeWeapon;
            
            SpawnText((WeaponView)weapon);
        }

        public void SpawnText(IEntity entity)
        {
            var moneyText = _moneyTextFactory.Next();
            moneyText.transform.position = _moneyTextSpawnPoint.position;
            ((MoneyTextView)moneyText).SetText($"+{entity.Entity.Price}$");
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