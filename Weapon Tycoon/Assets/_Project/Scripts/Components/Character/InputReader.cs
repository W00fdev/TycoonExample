using UnityEngine;

namespace _Project.Scripts.Components.Character
{
    /// <summary>
    /// Standing -> Moving -> Jumping (1 frame) -> Falling + Moving
    /// </summary>

    public class InputReader
    {
        private Vector3 _input;
        private bool _isJumping;

        public Vector3 Value => _input;
        public bool IsJumping => _isJumping;
        
        public void Update()
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = 0f;
            _input.z = Input.GetAxisRaw("Vertical");

            if (_input.magnitude > 1f)
                _input.Normalize();
            
            _isJumping = Input.GetKeyDown(KeyCode.Space);
        }
    }
}