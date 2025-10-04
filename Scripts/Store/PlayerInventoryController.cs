using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerInventoryController : Node
{
    private static PlayerInventoryController Instance;

    public static InventoryData Inventory => Instance.inventory;

    private InventoryData inventory = new InventoryData();

    public override void _Ready()
    {
        base._Ready();
        if (Instance != null)
        {
            Instance.QueueFree();
        }
        Instance = this;

        // TEMP
        for (int i = 0; i < 10; i++)
        {
            inventory.Cards.Add(new InventoryIDCard(CardsLoader.RandomCard().ID));
        }
    }

    public static void AddCard(AInventoryCard card) => Instance.inventory.Cards.Add(card);
    public static void LoseCard(AInventoryCard card) => Instance.inventory.Cards.Remove(card);

    public static void TrySell(Action onSuccess)
    {
        // TBA: Confirm box
        onSuccess();
    }
}
