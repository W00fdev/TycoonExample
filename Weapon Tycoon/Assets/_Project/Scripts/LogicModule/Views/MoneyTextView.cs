using TMPro;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Views
{
    public class MoneyTextView : PooledView
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TextAnimation _textAnimation;

        public void PlayTextAnimation() => _textAnimation?.Play();
        public void SetText(string text) => _text.text = text;
    }
}