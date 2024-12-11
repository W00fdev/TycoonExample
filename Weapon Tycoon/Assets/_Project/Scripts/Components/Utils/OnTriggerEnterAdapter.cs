using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Components.Utils
{
    public class OnTriggerEnterAdapter : MonoBehaviour
    {
        [SerializeField] private UnityEvent _actionCompleted;
        [SerializeField] private LayerMask _layer;

        private void OnTriggerEnter(Collider other)
        {
            if ((_layer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                _actionCompleted?.Invoke();
            }
        }
    }
}