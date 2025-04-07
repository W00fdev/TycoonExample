using System;
using _Project.Scripts.Components;
using _Project.Scripts.LogicModule;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Data
{
    [Serializable]
    public class EnemySceneReferencesDTO
    {
        public Health WallHealth;
        public VolumePivot WallPivot;
        public VolumePivot FlagPivot;
    }
}