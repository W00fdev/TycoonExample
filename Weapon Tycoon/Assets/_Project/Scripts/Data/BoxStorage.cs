using System;
using System.Collections.Generic;
using _Project.Scripts.LogicModule.Views;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "BoxStorage", menuName = "Config/BoxPrefabs")]
    public class BoxStorage : ScriptableObject
    {
        [Serializable]
        public enum BoxType
        {
            Box, LongBox
        }

        [Serializable]
        public class BoxKeyValue
        {
            public BoxType Type;
            public PooledView Box;
        }
        
        public List<BoxKeyValue> Boxes;
    }
}