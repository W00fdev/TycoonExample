using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class ButtonSpawnerBuyer : MonoBehaviour
    {
        public void BuySpawner()
        {
            EventBus.BuyNextSpawnerPressed?.Invoke();
        }
    }
}