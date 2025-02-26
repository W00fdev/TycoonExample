using _Project.Scripts.LocalizationSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views.Turrets
{
    public class TurretBuyerInfoView : MonoBehaviour
    {
        [SerializeField] private LangText _name;
        [SerializeField] private TMP_Text _fireRateText;
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private TMP_Text _turretPriceText;

        public void Initialize(string spawnerKeyName, string turretPrice, string fireRate, string damage)
        {
            _name.ChangeKey(spawnerKeyName);
            
            _turretPriceText.text = turretPrice;
            _damageText.text = damage;
            _fireRateText.text = fireRate;
        }

        public void DisableSelf()
            => gameObject.SetActive(false);
    }
}