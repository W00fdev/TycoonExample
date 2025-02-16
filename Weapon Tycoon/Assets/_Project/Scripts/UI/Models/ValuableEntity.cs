using UnityEngine;

namespace _Project.Scripts.UI.Models
{
    [CreateAssetMenu(fileName = "Valuable Entity", menuName = "Config/Valuable Entities")]
    public class ValuableEntity : ScriptableObject
    {
        public int SpawnerGrade;
    }
}