using UnityEngine;
using Zenject;

namespace _Project.Scripts.Components.Character
{
    // No interface by RAP (repeat abstraction principle)
    public class InputReader : ITickable
    {
        private Vector3 _input = Vector3.zero;
        private bool _isJumping;

        public Vector3 Value => _input;
        public bool IsJumping => _isJumping;

        public bool IsLeftMouseButton => Input.GetMouseButton(0);
        
        public void Tick()
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = 0f;
            _input.z = Input.GetAxisRaw("Vertical");
            _input.Normalize();
            
            _isJumping = Input.GetKeyDown(KeyCode.Space);
        }
    }
}