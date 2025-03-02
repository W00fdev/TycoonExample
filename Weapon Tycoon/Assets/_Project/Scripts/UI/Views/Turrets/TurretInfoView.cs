using _Project.Scripts.LocalizationSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views.Turrets
{
    public class TurretInfoView : MonoBehaviour
    {
        [SerializeField] private LangText _name;
        [SerializeField] private TMP_Text _fireRateText;
        [SerializeField] private TMP_Text _damageText;

        public void Initialize(string keyName)
            => _name.ChangeKey(keyName);

        public void UpdateInfo(string fireRate, string damage)
        {
            _fireRateText.text = fireRate;
            _damageText.text = damage;
        }
    }
}