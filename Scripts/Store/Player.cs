using Godot;
using System;

public partial class Player : Sprite2D
{
    [Export] private Texture2D[] sprites;
    [Export] private int[] thresolds;

    public override void _Ready()
    {
        base._Ready();
        Stats.OnCardSold += OnSoldCard;
    }

    private void OnSoldCard()
    {
        int amount = Stats.CardsSold;
        int index = 0;
        while (amount >= thresolds[index] && index + 1 < thresolds.Length) index++;
        Texture = sprites[index];
    }
}
