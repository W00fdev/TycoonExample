using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views
{
    public class SpawnerBuyerInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _productPriceText;
        [SerializeField] private TMP_Text _spawnerPriceText;

        public void Initialize(string spawnerName, string spawnerPrice, string speed, string productPrice)
        {
            _name.text = spawnerName;
            _spawnerPriceText.text = spawnerPrice;
            _productPriceText.text = productPrice;
            _speedText.text = speed;
        }

        public void DisableSelf()
            => gameObject.SetActive(false);
    }
}