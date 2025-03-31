using _Project.Scripts.Infrastructure.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.UI.Views
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] protected PopupDialog _uiPopup;
        
        public void ShowInZone()
        {
            if (gameObject.activeInHierarchy)
                _uiPopup.ShowAsync().Forget();
        }

        public void HideInZone()
        {
            if (gameObject.activeInHierarchy)
                _uiPopup.HideAsync().Forget();
        }
    }
}