using _Project.Scripts.LogicModule.BigBeautifulWall;
using _Project.Scripts.LogicModule.Spawners;
using _Project.Scripts.LogicModule.Turrets;
using _Project.Scripts.LogicModule.Waves;
using _Project.Scripts.UI.Presenters;
using UnityEngine;

namespace _Project.Scripts.LogicModule
{
    public class EconomyShop : MonoBehaviour
    {
        [SerializeField] private CurrencyPipe _currencyPipe;
        [SerializeField] private SpawnersController _spawnersController;
        
        public void Initialize()
        {
            _spawnersController.Initialize();
            
        }
    }
}