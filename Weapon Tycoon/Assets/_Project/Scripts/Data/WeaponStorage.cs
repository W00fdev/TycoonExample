using System;
using System.Collections.Generic;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "WeaponStorage", menuName = "Config/WeaponPrefabs")]
    public class WeaponStorage : ScriptableObject
    {
        [Serializable]
        public enum WeaponType
        {
            Pistol1, Shotgun1, Rifle1, Sniper1
        }

        [Serializable]
        public class WeaponKeyValue
        {
            public WeaponType Type;
            public WeaponView Weapon;
        }
        
        public List<WeaponKeyValue> Weapons;
    }
    
    
}