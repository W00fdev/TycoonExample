using _Project.Scripts.CurrencyModule.Models;
using _Project.Scripts.UI.Models;

namespace _Project.Scripts.CurrencyModule
{
    public interface IEntity
    {
        ValuableEntity Entity { get; }
    }
}