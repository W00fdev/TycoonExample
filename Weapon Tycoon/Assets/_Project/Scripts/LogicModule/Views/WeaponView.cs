using _Project.Scripts.Animations;
using _Project.Scripts.CurrencyModule;
using _Project.Scripts.CurrencyModule.Models;
using UnityEngine;

namespace _Project.Scripts.LogicModule.Views
{
    public class WeaponView : PooledView, IEntity
    {
        [SerializeField] private ValuableEntity _entity;

        public ValuableEntity Entity => _entity;
    }
}