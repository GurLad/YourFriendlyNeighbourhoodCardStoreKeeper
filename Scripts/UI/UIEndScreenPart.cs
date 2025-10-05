using Godot;
using System;

public partial class UIEndScreenPart : Node
{
    [Export] private Label what;
    [Export] private Label message;
    [Export] private Color[] colors;
    [Export] private float delay = 0.6f;

    private Interpolator interpolator = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
    }

    public void Clear()
    {
        what.Modulate = Colors.Transparent;
        message.Modulate = Colors.Transparent;
    }

    public void Show(string whatt, string messaget, int color, Action finish)
    {
        what.Text = whatt;
        message.Text = messaget;
        interpolator.Delay(delay);
        interpolator.OnFinish = () =>
        {
            what.Modulate = Colors.White;
            interpolator.Delay(delay);
            interpolator.OnFinish = () =>
            {
                message.Modulate = colors[Mathf.Clamp(color, 0, colors.Length - 1)];
                message.RotationDegrees = ExtensionMethods.RNG.Next(-30, 30);
                finish?.Invoke();
            };
        };
    }
}
