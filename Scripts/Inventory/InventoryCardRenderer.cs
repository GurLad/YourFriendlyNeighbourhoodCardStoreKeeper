using Godot;
using System;

public partial class InventoryCardRenderer : Node
{
    [Export] private CardRenderer cardRenderer;
    [Export] private Button buySellButton;
    [Export] private Label newLabel;

    private bool inited = false;
    private Func<bool> canPress = null;

    [Signal]
    public delegate void OnButtonPressedEventHandler();

    public void Render(CardData data, bool isNew)
    {
        cardRenderer.Render(data);
        newLabel.Visible = isNew;
    }

    public void InitButton(string text, Func<bool> canPress, Action onPressed)
    {
        if (inited)
        {
            GD.PushError("[InventoryCardRenderer]: Double init!");
            return;
        }
        inited = true;
        this.canPress = canPress;
        buySellButton.Text = text;
        buySellButton.Pressed += onPressed;
        buySellButton.Pressed += () => EmitSignal(SignalName.OnButtonPressed);
        UpdateCanPress();
    }

    public void UpdateCanPress() => buySellButton.Disabled = !canPress();
}
