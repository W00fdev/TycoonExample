using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views.BigBeautifulWall
{
    public class RepairInfoView : PopupView
    {
        [SerializeField] private TMP_Text _repairPriceText;

        public void SetPriceInfo(string repairPrice) 
            => _repairPriceText.text = repairPrice;

        public void EnableSelf()
            => gameObject.SetActive(true);
        
        public void DisableSelf()
            => gameObject.SetActive(false);
    }
}