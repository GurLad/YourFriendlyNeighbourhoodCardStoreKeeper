using Godot;
using System;
using System.Linq;

public partial class Player : Sprite2D
{
    [Export] private Table[] tables;
    [ExportCategory("Internal")]
    [Export] private Texture2D[] sprites;
    [Export] private int[] thresolds;
    [Export] private Vector2 robRate;

    private Interpolator interpolator = new Interpolator();
    private Timer robTimer = new Timer();

    private Action cancelAction;
    private ACustomer robbing = null;

    public override void _Ready()
    {
        base._Ready();
        Stats.OnCardSold += OnSoldCard;

        AddChild(interpolator);
        interpolator.InterruptMode = Interpolator.Mode.Error;

        AddChild(robTimer);
        robTimer.OneShot = true;
        robTimer.WaitTime = robRate.RandomValueInRange();
        robTimer.Timeout += RobOneCard;

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
        cancelAction = null;
        if (interpolator.Active)
        {
            interpolator.Stop(false);
        }
        if (robTimer.TimeLeft > 0 || !robTimer.Paused)
        {
            robTimer.Stop();
        }
        PreArrive(chair);
        Vector2 targetPos = chair.GlobalPosition +
            (GlobalPosition.Y < chair.GlobalPosition.Y ? 1 : -1) * Vector2.Up * 32;
        if (GlobalPosition.DistanceTo(targetPos) <= 0.01f)
        {
            OnArrive(chair);
            return;
        }
        interpolator.InterpolateMoveOnPath(this, 5, GlobalPosition, targetPos);
        interpolator.OnFinish = () => OnArrive(chair);
    }

    private void PreArrive(Chair chair)
    {
        if (!chair.IsEmpty)
        {
            if (chair.CustomerSitting)
            {
                // Trade
                chair.Customer.PrepareForTrade();
                cancelAction = chair.Customer.FinishTrade;
            }
        }
    }

    private void OnArrive(Chair chair)
    {
        cancelAction = null;
        LookAt(chair.GlobalPosition);
        RotationDegrees += 180;
        if (!chair.IsEmpty)
        {
            if (chair.CustomerSitting)
            {
                // TBA
                chair.Customer.FinishTrade();
                GD.Print("Traded");
            }
            else if (chair.Customer.Inventory.Cards.Count > 0)
            {
                // Rob
                robbing = chair.Customer;
                RobOneCard();
            }
        }
    }

    private ACustomer GetOpponent(ACustomer customer)
    {
        // So cursed
        Table table = tables.ToList().Find(a => a.chairs.ToList().Contains(customer.Chair));
        Chair chair = table.chairs.ToList().Find(a => a != customer.Chair);
        return chair.IsEmpty ? null : chair.Customer;
    }

    private void RobOneCard()
    {
        if (robbing.Inventory.Cards.Count <= 0) // Failsafe
        {
            robbing.Chair.ForceHideWallet();
            return;
        }
        AInventoryCard card = robbing.Inventory.Cards[0];
        robbing.Inventory.Cards.RemoveAt(0);
        PlayerInventoryController.AddCard(card);
        Stats.StealCard(null, null);
        if (robbing.CanSeeTheft(this, robbing) || (GetOpponent(robbing)?.CanSeeTheft(this, robbing) ?? false))
        {
            SoundController.Current.PlaySFX("Detected");
            Stats.TheftDetected();
        }
        else if (robbing.Inventory.Cards.Count <= 0)
        {
            SoundController.Current.PlaySFX("Steal");
            robbing.Chair.ForceHideWallet();
        }
        else
        {
            SoundController.Current.PlaySFX("Steal");
            robTimer.WaitTime = robRate.RandomValueInRange();
            robTimer.Start();
        }
        GD.Print("rob rob");
    }
}
