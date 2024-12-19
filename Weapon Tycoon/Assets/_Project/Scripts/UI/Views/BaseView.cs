using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        public virtual void UpdateCurrency(string amount) => _amountText.text = amount;
    }
}