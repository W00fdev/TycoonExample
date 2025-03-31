using System;
using _Project.Scripts.Infrastructure.ScriptableEvents;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace _Project.Scripts.Components.Character
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _player;
        
        [SerializeField] private float _sensitivity = 5f;
        [SerializeField] private Vector2 _cameraLimit = new Vector2(-45, 40);

        [SerializeField] private float _offsetDistanceY;
        [SerializeField] private float _offsetDistanceX = 3;
        [SerializeField] private float _distance;

        [SerializeField] private LayerMask _allExceptPlayer;
        [SerializeField] private float _smoothingStep;

        private float _mouseX;
        private float _mouseY;
        private float _offsetZ;
        private bool _movementAllowed = true;

        // Context Invokation
        public void StopCameraMovement(Empty _) 
            => _movementAllowed = false;

        // Context Invokation
        public void ResumeCameraMovement(Empty _) 
            => _movementAllowed = true;

        void LateUpdate()
        {
            if (!_movementAllowed)
                return;
            
            _mouseX += Input.GetAxis("Mouse X") * _sensitivity;
            _mouseY += Input.GetAxis("Mouse Y") * _sensitivity;
            _mouseY = Mathf.Clamp(_mouseY, _cameraLimit.x, _cameraLimit.y);

            _camera.transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0);

            var position = _camera.transform.rotation * new Vector3(_offsetDistanceX, 0, -_distance);
            position += _player.position;
            position += Vector3.up * _offsetDistanceY;
            
            Ray backSpaceCamera = new Ray(_player.position, -_camera.transform.forward);
            float distanceToCamera = Vector3.Distance(_player.position, _camera.transform.position);
            float distanceToWall = distanceToCamera;
            
            if (Physics.Raycast(backSpaceCamera, out var hit, distanceToCamera + 0.1f, _allExceptPlayer.value))
                distanceToWall = Vector3.Distance(_player.position, hit.point);

            _offsetZ = Mathf.SmoothStep(_offsetZ, Mathf.Abs(distanceToWall - distanceToCamera), _smoothingStep * Time.deltaTime);
            if (distanceToWall <= 10f)
                position += _camera.transform.forward * _offsetZ;
            
            _camera.transform.position = position;
        }
    }
}