using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.CurrencyModule.Views
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        public void UpdateCurrency(string amount) => _amountText.text = amount;
    }
}