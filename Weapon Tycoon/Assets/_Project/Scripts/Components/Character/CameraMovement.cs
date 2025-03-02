using _Project.Scripts.Infrastructure.ScriptableEvents;
using UnityEngine;

namespace _Project.Scripts.Components.Character
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public float sensitivity = 5f;
        public Vector2 cameraLimit = new Vector2(-45, 40);

        float mouseX;
        float mouseY;
        public float offsetDistanceY;
        public float offsetDistanceX = 0;
        public float distance;

        [SerializeField] private LayerMask _allExceptPlayer;
        [SerializeField] private float _smoothingStep;
        
        private Transform player;
        private RaycastHit[] _anyCollider;
        private float _offsetZ;
        private bool _movementAllowed;

        void Start()
        {
            _anyCollider = new RaycastHit[1];
            player = GameObject.FindWithTag("Player").transform;

            _movementAllowed = true;
        }

        // Context Invokation
        public void StopCameraMovement(Empty _)
        {
            _movementAllowed = false;
        }
        
        // Context Invokation
        public void ResumeCameraMovement(Empty _)
        {
            _movementAllowed = true;
        }
        
        void LateUpdate()
        {
            if (!_movementAllowed)
                return;
            
            mouseX += Input.GetAxis("Mouse X") * sensitivity;
            mouseY += Input.GetAxis("Mouse Y") * sensitivity;
            mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

            _camera.transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);

            var position = _camera.transform.rotation * new Vector3(0f, 0, -distance);
            position += player.position;
            position += Vector3.up * offsetDistanceY;
            
            Ray backSpaceCamera = new Ray(player.position, -_camera.transform.forward);
            float distanceToCamera = Vector3.Distance(player.position, _camera.transform.position);
            float distanceToWall = distanceToCamera;
            
            if (Physics.RaycastNonAlloc(backSpaceCamera, _anyCollider, distanceToCamera + 0.1f, _allExceptPlayer.value) > 0)
                distanceToWall = Vector3.Distance(player.position, _anyCollider[0].point);

            _offsetZ = Mathf.SmoothStep(_offsetZ, Mathf.Abs(distanceToWall - distanceToCamera), _smoothingStep * Time.deltaTime);
            if (distanceToWall <= 10f)
                position += _camera.transform.forward * _offsetZ;
            
            _camera.transform.position = position;
        }
    }
}