using _Project.Scripts.Animations;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.UI.Models;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Views
{
    public class BlasterView : PooledView, IEntity
    {
        [SerializeField] private ValuableEntity _entity;

        public ValuableEntity Entity => _entity;
    }
}