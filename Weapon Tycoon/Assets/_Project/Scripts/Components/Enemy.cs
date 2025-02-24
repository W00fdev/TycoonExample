using PrimeTween;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform _basicModel;
        [SerializeField] private Color _damagedColor;

        public void TakeDamage()
        {
            Tween.PunchScale(_basicModel, Vector3.up * 0.1f, 0.1f);
        }
    }
}