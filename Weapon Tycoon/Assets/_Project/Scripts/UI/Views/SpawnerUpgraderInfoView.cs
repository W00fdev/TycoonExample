using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI.Views
{
    public class SpawnerUpgraderInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _upgradePriceText ;

        public void SetPriceInfo(string upgradePrice)
        {
            _upgradePriceText.text = upgradePrice;
        }
        
        public void EnableSelf()
            => gameObject.SetActive(true);
    }
}