using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.LogicModule
{
    [Serializable]
    public class VolumePivot : MonoBehaviour
    {
        [SerializeField] private Transform _center;
        [SerializeField] private Vector3 _halfExtents;

        public Vector3 Center => _center.position;
        public Vector3 HalfExtents => _halfExtents;
        
        public Vector3 RandomInsideVolume()
            => _center.position + new Vector3(
                _halfExtents.x * (2f * UnityEngine.Random.value - 1f),
                _halfExtents.y * (2f * UnityEngine.Random.value - 1f),
                _halfExtents.z * (2f * UnityEngine.Random.value - 1f));
        
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_center != null)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(_center.position, _halfExtents);
            }
        }
#endif
    }
}