using System;
using UnityEngine;

namespace _Project.Scripts.Animations
{
    public class ConveyorMovable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _conveyorSpeed;

        public void SetSpeed(float speed)
        {
            if (speed <= 0f)
                throw new ArgumentException("Conveyor speed can't be <= 0f");
        
            _conveyorSpeed = speed;
        }

        private void OnCollisionStay(Collision conveyor)
        {
            if (string.Compare(conveyor.gameObject.tag, "Conveyor", StringComparison.Ordinal) == 0)
            {
                _rigidbody.linearVelocity = conveyor.transform.forward * _conveyorSpeed;
            }
        }
    
        /*private void OnCollisionExit(Collision other)
    {
        if (string.Compare(other.gameObject.tag, "Conveyor", StringComparison.Ordinal) == 0)
        {
        }
    }*/
        public void ClearInertia()
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}
