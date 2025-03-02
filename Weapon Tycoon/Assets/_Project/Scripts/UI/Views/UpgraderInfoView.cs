using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views.Spawners
{
    public class UpgraderInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _upgradePriceText ;

        public void SetPriceInfo(string upgradePrice) 
            => _upgradePriceText.text = upgradePrice;

        public void EnableSelf()
            => gameObject.SetActive(true);
    }
}