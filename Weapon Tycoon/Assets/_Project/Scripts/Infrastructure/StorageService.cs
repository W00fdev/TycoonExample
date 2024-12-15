using _Project.Scripts.Data;
using _Project.Scripts.LogicModule.Views;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class StorageService
    {
        private readonly WeaponStorage _weaponStorage;
        private readonly BoxStorage _boxStorage;
        
        public StorageService()
        {
            _weaponStorage = Resources.Load<WeaponStorage>(AssetPath.StoragesPath + "WeaponStorage");
            _boxStorage = Resources.Load<BoxStorage>(AssetPath.StoragesPath + "BoxStorage");
        }

        public WeaponView GetWeaponView(WeaponStorage.WeaponType type) =>
            _weaponStorage.Weapons.Find(x => x.Type == type).Weapon;
        
        public PooledView GetBoxView(BoxStorage.BoxType type) => _boxStorage.Boxes.Find(x => x.Type == type).Box;
    }
}