using Godot;
using System;

public partial class MoneyController : Node
{
    private static MoneyController Instance;

    public static int CurrentAmount => Instance.amount;

    private int amount = 5;

    public override void _Ready()
    {
        base._Ready();
        if (Instance != null)
        {
            Instance.QueueFree();
        }
        Instance = this;
    }

    public static void SpendMoney(int amount) => Instance.amount -= amount;
    public static void GainMoney(int amount) => Instance.amount += amount;
}
