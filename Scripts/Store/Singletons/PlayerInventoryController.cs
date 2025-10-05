using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerInventoryController : Node
{
    private static PlayerInventoryController Instance;

    public static InventoryData Inventory => Instance.inventory;
    public static event Action GainedCard;

    private InventoryData inventory = new InventoryData();

    public override void _Ready()
    {
        base._Ready();
        if (Instance != null)
        {
            Instance.QueueFree();
        }
        Instance = this;
    }

    public static int CountUnique()
    {
        int count = 0;
        for (int i = 0; i < CardsLoader.Count; i++)
        {
            if (Inventory.Cards.FindIndex(a => a is InventoryIDCard id && id.ID == i) >= 0) count++;
        }
        return count;
    }

    public static int CountUniqueFoil()
    {
        int count = 0;
        for (int i = 0; i < CardsLoader.Count; i++)
        {
            if (Inventory.Cards.FindIndex(a => a is InventoryIDCard id && id.ID == i && id.Foil) >= 0) count++;
        }
        return count;
    }

    public static void AddCard(AInventoryCard card) { Instance.inventory.Cards.Add(card); GainedCard?.Invoke(); }
    public static void LoseCard(AInventoryCard card) => Instance.inventory.Cards.Remove(card);

    public static void TrySell(Action onSuccess)
    {
        // TBA: Confirm box
        onSuccess();
    }
}
