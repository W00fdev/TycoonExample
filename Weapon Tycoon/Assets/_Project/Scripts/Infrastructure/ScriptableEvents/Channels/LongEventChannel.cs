using UnityEngine;

namespace _Project.Scripts.Infrastructure.ScriptableEvents.Channels
{
    [CreateAssetMenu(menuName = "Events/CurrencyEventChannel")]
    public class CurrencyEventChannel : EventChannel<long> {}
}