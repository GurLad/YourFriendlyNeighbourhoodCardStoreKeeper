using Godot;
using System;

public partial class UICollectionButton : Button
{
    [Export] private Control displayFull;
    [Export] private AInventoryRenderer display;
    [Export] private float openCloseTime = 0.5f;

    private Interpolator interpolator = new Interpolator();
    private bool visible = false;

    public override void _Ready()
    {
        base._Ready();
        Pressed += TryOpenClose;

        displayFull.PivotOffset = new Vector2(504.0f, 352.0f) / 2;
        Text = PlayerInventoryController.CountUnique() + "/40";
        AddChild(interpolator);
        PlayerInventoryController.GainedCard += () => Text = PlayerInventoryController.CountUnique() + "/40";
    }

    private void TryOpenClose()
    {
        if (!visible)
        {
            display.Render(PlayerInventoryController.Inventory);
            display.Visible = true;
            display.AnimateShow();
            visible = true;
        }
        else
        {
            display.AnimateHide();
            visible = false;
        }
    }

    private void TryOpenClose2()
    {
        if (interpolator.Active)
        {
            GD.PushWarning("Wah");
            return;
        }
        bool hide = false;
        Disabled = true;
        display.Render(PlayerInventoryController.Inventory);
        if (displayFull.Visible)
        {
            interpolator.Interpolate(openCloseTime,
                new Interpolator.InterpolateObject(
                    a => displayFull.Scale = Vector2.One * a,
                    displayFull.Scale.X,
                    0,
                    Easing.EaseInBack
                ));
            hide = true;
        }
        else
        {
            interpolator.Interpolate(openCloseTime,
                new Interpolator.InterpolateObject(
                    a => displayFull.Scale = Vector2.One * a,
                    displayFull.Scale.X,
                    1,
                    Easing.EaseOutBack
                ));
            displayFull.Visible = true;
        }
        interpolator.OnFinish = () =>
        {
            Disabled = false;
            if (hide)
            {
                displayFull.Visible = false;
            }
        };
    }
}
