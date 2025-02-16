using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI.Views
{
    public class SpawnerUpgraderInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _upgradePriceText ;
        [SerializeField] private TMP_Text _upgradeValueText;
        [SerializeField] private TMP_Text _beforeValueText;

        public void UpdateInfo(string upgradePrice, string upgradeValue)
        {
            _beforeValueText.text = _upgradeValueText.text;
            
            _upgradePriceText.text = upgradePrice;
            _upgradeValueText.text = upgradeValue;
        }
        
        public void SetBeforeInfo(string upgradeValue)
            => _beforeValueText.text = upgradeValue;

        public void EnableSelf()
            => gameObject.SetActive(true);
    }
}