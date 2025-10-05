using Godot;
using System;

public partial class UITooltip : FadeTransition
{
    [Export] private Label Label;

    private bool shown = false;

    public void SetText(string text)
    {
        Label.Text = text;
    }

    public void ShowTooltip(string text, float delay = 0)
    {
        shown = true;
        if (delay <= 0 || interpolator.Active)
        {
            SetText(text);
            TransitionIn();
        }
        else
        {
            interpolator.Delay(delay);
            interpolator.OnFinish = () => ShowTooltip(text, 0);
        }
    }

    public void HideTooltip()
    {
        if (!shown)
        {
            return;
        }
        shown = false;
        TransitionOut();
    }
}
