using Godot;
using System;

public partial class InventoryCardRenderer : Control
{
    [Export] private CardRenderer cardRenderer;
    [Export] private Button buySellButton;
    [Export] private Label newLabel;

    public AInventoryCard Card { get; private set; }

    private bool inited = false;
    private Func<bool> canPress = null;

    [Signal]
    public delegate void OnButtonPressedEventHandler();

    public void Render(AInventoryCard card, bool isNew)
    {
        cardRenderer.Render((Card = card).Data);
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
