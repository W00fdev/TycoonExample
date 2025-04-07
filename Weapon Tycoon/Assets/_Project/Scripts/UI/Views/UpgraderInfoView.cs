using _Project.Scripts.Infrastructure.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views.Spawners
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