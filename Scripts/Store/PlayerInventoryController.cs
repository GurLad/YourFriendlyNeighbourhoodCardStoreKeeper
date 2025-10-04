using Godot;
using System;

public partial class PlayerInventoryController : Node
{
    private static PlayerInventoryController Instance;

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

    public static void AddCard(AInventoryCard card) => Instance.inventory.Cards.Add(card);
    public static void LoseCard(AInventoryCard card) => Instance.inventory.Cards.Remove(card);
}
