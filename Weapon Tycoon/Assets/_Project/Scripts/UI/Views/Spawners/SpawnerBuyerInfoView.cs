using _Project.Scripts.LocalizationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI.Views.Spawners
{
    public class SpawnerBuyerInfoView : MonoBehaviour
    {
        [SerializeField] private LangText _name;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _productPriceText;
        [SerializeField] private TMP_Text _spawnerPriceText;

        public void Initialize(string spawnerKeyName, string spawnerPrice, string speed, string productPrice)
        {
            _name.ChangeKey(spawnerKeyName);
            
            _spawnerPriceText.text = spawnerPrice;
            _productPriceText.text = productPrice;
            _speedText.text = speed;
        }

        public void DisableSelf()
            => gameObject.SetActive(false);
    }
}