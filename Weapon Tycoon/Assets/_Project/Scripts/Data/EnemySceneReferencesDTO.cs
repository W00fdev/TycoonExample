using System;
using _Project.Scripts.Components;
using _Project.Scripts.LogicModule;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Data
{
    [Serializable]
    public class EnemySceneReferencesDTO
    {
        [FormerlySerializedAs("WallHealthable")] [FormerlySerializedAs("WallHealth")] public HealthComponent WallHealthComponent;
        public VolumePivot WallPivot;
        public VolumePivot FlagPivot;
    }
}