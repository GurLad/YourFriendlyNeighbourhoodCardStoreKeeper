using Godot;
using System;

public partial class InventoryCardRenderer : Control
{
    [Export] private CardRenderer cardRenderer;
    [Export] private Button buySellButton;
    [Export] private Label buySellButtonLabel;
    [Export] private RichTextLabel newLabel;

    private AInventoryCard card;
    public AInventoryCard Card
    {
        get
        {
            if (card == null)
            {
                GD.PushError("[InventoryCardRenderer]: Trying to get cardless renderer data!");
            }
            return card;
        }
        private set => card = value;
    }
    private CardData data;
    public CardData Data
    {
        get => Card.Data ?? data;
        private set => data = value;
    }

    private bool inited = false;
    private Func<bool> canPress = null;

    [Signal]
    public delegate void OnButtonPressedEventHandler();

    public void Render(AInventoryCard card, bool isNew)
    {
        cardRenderer.Render((Card = card).Data);
        newLabel.Visible = isNew;
    }

    public void Render(CardData data, bool isNew)
    {
        cardRenderer.Render(Data = data);
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
        buySellButtonLabel.Text = text;
        buySellButton.Pressed += onPressed;
        buySellButton.Pressed += () => EmitSignal(SignalName.OnButtonPressed);
        UpdateCanPress();
    }

    public void UpdateCanPress() => buySellButton.Disabled = !canPress();

    public void UpdateButtonText(string newVal) => buySellButtonLabel.Text = newVal;
}
