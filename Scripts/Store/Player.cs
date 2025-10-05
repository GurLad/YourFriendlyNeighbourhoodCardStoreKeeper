using Godot;
using System;
using System.Linq;

public partial class Player : Sprite2D
{
    [Export] private Table[] tables;
    [ExportCategory("Internal")]
    [Export] private Texture2D[] sprites;
    [Export] private int[] thresolds;

    private Interpolator interpolator = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        Stats.OnCardSold += OnSoldCard;
        AddChild(interpolator);

        tables.ToList().ForEach(a => a.chairs.ToList().ForEach(a => a.OnLeftClick += OnLeftClick));
    }

    private void OnSoldCard()
    {
        int amount = Stats.CardsSold;
        int index = 0;
        while (amount >= thresolds[index] && index + 1 < thresolds.Length) index++;
        Texture = sprites[index];
    }

    private void OnLeftClick(Chair chair)
    {
        // Temp
        interpolator.InterpolateMoveOnPath(this, 5, GlobalPosition, chair.GlobalPosition + Vector2.Up * 32);
    }
}
