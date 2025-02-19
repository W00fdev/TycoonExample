using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.UI.Views
{
    public class CashStacksView : MonoBehaviour
    {
        [SerializeField, ReadOnly] private int  StacksCount = 12;
        [SerializeField] private GameObject[] _cashStacks;

        private int _prevActiveStacks = 13;
        
        public void UpdateCurrency(int amount)
        {
            int activeStacksCount = CalculateCountOfStacks(amount);
            
            for (int i = activeStacksCount; i < _cashStacks.Length; i++)
                _cashStacks[i].SetActive(false);
            
            for (int i = _prevActiveStacks; i < activeStacksCount + 1; i++)
                _cashStacks[i].SetActive(true);
            
            _prevActiveStacks = activeStacksCount;
        }

        private int CalculateCountOfStacks(int amount)
        {
            return amount switch
            {
                > 20000 => 16,
                > 10000 => 15,
                > 4000 => 14,
                > 2000 => 13,
                > 1000 => 12,
                > 120 => 11,
                > 110 => 10,
                > 100 => 9,
                > 90 => 8,
                > 80 => 7,
                > 70 => 6,
                > 60 => 5,
                > 50 => 4,
                > 40 => 3,
                > 30 => 2,
                >= 20 => 1,
                < 20 => 0
            };
        }
    }
}