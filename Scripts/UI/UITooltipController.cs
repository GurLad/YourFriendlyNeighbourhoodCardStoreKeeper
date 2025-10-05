using Godot;
using System;

public partial class UITooltipController : Node
{
    public static UITooltipController Current { get; private set; }

    [Export] private UITooltip TooltipUpright;
    [Export] private UITooltip TooltipSideways;
    [Export] private float delay = 0.1f;

    public override void _Ready()
    {
        base._Ready();
        Current = this;
    }

    public void ShowTooltip(Vector2 pos, string text, bool upright)
    {
        ShowTooltip(pos, Vector2.Zero, text, upright);
    }

    public void ShowTooltip(Sprite2D source, string text, bool upright)
    {
        ShowTooltip(source.GlobalPosition, source.Texture.GetSize(), text, upright);
    }

    public void ShowTooltip(Control source, string text, bool upright)
    {
        ShowTooltip(source.GlobalPosition, source.Size, text, upright);
    }

    private void ShowTooltip(Vector2 pos, Vector2 size, string text, bool upright)
    {
        UITooltip tooltip = upright ? TooltipUpright : TooltipSideways;
        tooltip.GlobalPosition = pos + tooltip.Size / 2 - size -
            (upright ?
                tooltip.Size.X * Vector2.Right / 2 :
                tooltip.Size.Y * Vector2.Up / 2);
        if (!upright)
        {
            tooltip.GlobalPosition += size / 2;
        }
        tooltip.ShowTooltip(text, delay);
    }

    public void HideTooltip()
    {
        TooltipUpright.HideTooltip();
        TooltipSideways.HideTooltip();
    }
}
