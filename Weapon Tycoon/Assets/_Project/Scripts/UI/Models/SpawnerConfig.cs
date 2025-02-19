﻿using _Project.Scripts.CurrencyModule.Models;
using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [CreateAssetMenu(fileName = "Spawner", menuName = "Config/Spawners")]
    public class SpawnerConfig : ScriptableObject
    {
        public float SpawnerSpeed;
        public int ProductPrice;
        public int BuyPrice;
    }
}