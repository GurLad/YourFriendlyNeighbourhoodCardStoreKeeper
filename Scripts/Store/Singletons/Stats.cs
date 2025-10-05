using Godot;
using System;

public static class Stats
{
    public static int CardsSold { get; private set; }

    public static event Action OnCardSold;

    public static void SellCards(int amount)
    {
        CardsSold += amount;
        OnCardSold?.Invoke();
    }
}
