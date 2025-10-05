using Godot;
using System;

public static class Stats
{
    public static int CardsSold { get; private set; } = 0;
    public static int CardsStolen { get; private set; } = 0;

    public static event Action OnCardSold;
    public static event Action OnTheftDetected;

    public static void SellCards(int amount)
    {
        CardsSold += amount;
        OnCardSold?.Invoke();
    }

    public static void StealCard(ACustomer target, ACustomer opponent)
    {
        CardsStolen++;
    }

    public static void TheftDetected()
    {
        OnTheftDetected?.Invoke();
    }
}
