using UnityEngine;

namespace _Project.Scripts.Animations
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ScannerAnimate : MonoBehaviour
    {
        [SerializeField] private float _conveyorSpeed = 1.2f;
        
        private MeshRenderer _meshRenderer;
        private Material _conveyorMaterial;
        
        private Vector2 _conveyorOffset;
        
        void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            
            _conveyorMaterial = _meshRenderer.sharedMaterial;
            _conveyorOffset = _conveyorMaterial.mainTextureOffset;
        }

        void Update()
        {
            _conveyorOffset.x -= _conveyorSpeed * Time.deltaTime;
            _conveyorMaterial.mainTextureOffset =_conveyorOffset;
        }
    }
}
