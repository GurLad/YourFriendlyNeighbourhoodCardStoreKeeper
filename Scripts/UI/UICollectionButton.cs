using Godot;
using System;

public partial class UICollectionButton : Button
{
    [Export] private Control displayFull;
    [Export] private AInventoryRenderer display;
    [Export] private float openCloseTime = 0.5f;

    private Interpolator interpolator = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        Pressed += TryOpenClose;

        Text = PlayerInventoryController.CountUnique() + "/40";
        AddChild(interpolator);
        PlayerInventoryController.GainedCard += () => Text = PlayerInventoryController.CountUnique() + "/40";
    }

    private void TryOpenClose()
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
                    1,
                    Easing.EaseInBack
                ));
            displayFull.Visible = true;
        }
        else
        {
            interpolator.Interpolate(openCloseTime,
                new Interpolator.InterpolateObject(
                    a => displayFull.Scale = Vector2.One * a,
                    displayFull.Scale.X,
                    0,
                    Easing.EaseOutBack
                ));
            hide = true;
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
