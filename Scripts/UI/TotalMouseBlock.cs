using Godot;
using System;

public partial class TotalMouseBlock : Control
{
    private static TotalMouseBlock Instance;

    public override void _Ready()
    {
        base._Ready();
        if (Instance != null)
        {
            QueueFree();
            return;
        }
        Instance = this;
        Unblock();
    }

    public static void Block() => Instance.Visible = true;
    public static void Unblock() => Instance.Visible = false;
}
