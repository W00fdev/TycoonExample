using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views
{
    public class UpgraderInfoView : PopupView
    {
        [SerializeField] private TMP_Text _upgradePriceText ;

        public void SetPriceInfo(string upgradePrice) 
            => _upgradePriceText.text = upgradePrice;

        public void EnableSelf()
            => gameObject.SetActive(true);
        
        public void DisableSelf()
            => gameObject.SetActive(false);
    }
}