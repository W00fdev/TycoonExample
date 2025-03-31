using _Project.Scripts.LogicModule.BigBeautifulWall;
using _Project.Scripts.LogicModule.Turrets;
using _Project.Scripts.LogicModule.Waves;
using _Project.Scripts.UI.Presenters;
using UnityEngine;

namespace _Project.Scripts.LogicModule
{
    public class DefenseShop : MonoBehaviour
    {
        [SerializeField] private CurrencyPipe _currencyPipe;
        
        [SerializeField] private TurretsController _turretsController;
        [SerializeField] private WavesController _wavesController;
        [SerializeField] private WallController _wallController;
        
        public void Initialize()
        {
            _turretsController.Initialize();
            _wavesController.Initialize();
            
            _wallController.Initialize();
        }
    }
}