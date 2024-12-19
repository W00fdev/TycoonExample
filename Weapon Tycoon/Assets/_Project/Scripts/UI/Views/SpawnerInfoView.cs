using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views
{
    public class SpawnerInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _priceText;

        public void Initialize(string name)
            => _name.text = name;

        public void UpdateInfo(string speed, string price)
        {
            _speedText.text = speed;
            _priceText.text = price;
        }
    }
}