using System;
using UnityEngine;

namespace _Project.Scripts.Components.Utils
{
    public class Billboard : MonoBehaviour
    {
        private Transform _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.forward = _mainCamera.forward;
        }
    }
}