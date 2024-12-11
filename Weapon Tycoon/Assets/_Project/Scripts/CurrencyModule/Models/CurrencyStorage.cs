using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Currency
{
    protected int _money;
    public int Money => _money;

    public Currency(int money)
    {
        _money = money;
    }
    
    [Button( "Add Money")]
    public void AddCurrency(int amount)
    {
        _money += amount;
    }
}