using Godot;
using System;
using System.Linq;

public partial class Player : Sprite2D
{
    [Export] private Table[] tables;
    [ExportCategory("Internal")]
    [Export] private Texture2D[] sprites;
    [Export] private int[] thresolds;
    [Export] private int robRate;

    private Interpolator interpolator = new Interpolator();
    private Action cancelAction;

    public override void _Ready()
    {
        base._Ready();
        Stats.OnCardSold += OnSoldCard;
        AddChild(interpolator);
        interpolator.InterruptMode = Interpolator.Mode.Error;

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
        if (cancelAction != null)
        {
            cancelAction();
        }
        if (interpolator.Active)
        {
            interpolator.Stop(false);
        }
        interpolator.InterpolateMoveOnPath(this, 5, GlobalPosition, chair.GlobalPosition + Vector2.Up * 32);
        if (!chair.IsEmpty)
        {
            if (chair.CustomerSitting)
            {
                // Trade
                chair.Customer.PrepareForTrade();
                cancelAction = chair.Customer.FinishTrade;
                interpolator.OnFinish = () =>
                {
                    // TBA
                    chair.Customer.FinishTrade();
                };
            }
            else if (chair.Customer.Inventory.Cards.Count > 0)
            {
                // Rob
                interpolator.OnFinish = () =>
                {
                    // TBA
                };
            }
        }
    }

    private Table TableFromChair(Chair chair)
    {
        // So cursed
        return tables.ToList().Find(a => a.chairs.ToList().Contains(chair));
    }
}
