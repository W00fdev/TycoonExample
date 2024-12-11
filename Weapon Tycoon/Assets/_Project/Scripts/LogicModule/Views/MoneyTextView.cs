using TMPro;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Views
{
    public class MoneyTextView : PooledView
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string text) => _text.text = text;
    }
}