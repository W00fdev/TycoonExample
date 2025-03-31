using _Project.Scripts.Infrastructure.Data;
using _Project.Scripts.UI.Views.BigBeautifulWall;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LogicModule.BigBeautifulWall
{
    public class WallController : MonoBehaviour
    {
        [SerializeField] private WallUpgrader _wallUpgrader;
        [SerializeField] private WallHealthView _wallHealthView;

        [Inject] private PersistentProgress _progress;
        
        public void Initialize()
        {
            _wallUpgrader.Initialize();

            if (_progress.Data.WallUpgrades != -1)
                _wallHealthView.ShowAsync().Forget();
            else
                _wallHealthView.HideInstant();
        }
    }
}