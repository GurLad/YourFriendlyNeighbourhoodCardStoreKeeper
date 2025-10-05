using Godot;
using System;

public partial class Chair : Sprite2D
{
    [Export] public Vector2 PosMod { get; set; } = new Vector2(5, 5);
    [Export] private ChairControl chairControl;
    [Export] private Sprite2D walletGraphic;

    public bool IsEmpty => Customer == null;
    public bool CustomerSitting { get; private set; } = true;
    public ACustomer Customer { get; private set; } = null;

    [Signal]
    public delegate void OnCustomerAttachedEventHandler(ACustomer customer);

    [Signal]
    public delegate void OnCustomerPausedEventHandler(ACustomer customer);

    [Signal]
    public delegate void OnCustomerResumedEventHandler(ACustomer customer);

    [Signal]
    public delegate void OnCustomerDetachedEventHandler(ACustomer customer);

    [Signal]
    public delegate void OnLeftClickEventHandler(Chair chair);

    [Signal]
    public delegate void OnRightClickEventHandler(Chair chair);

    public override void _Ready()
    {
        base._Ready();
        chairControl.Chair = this;
        if (FlipH)
        {
            PosMod = new Vector2(-PosMod.X, PosMod.Y);
            walletGraphic.Offset = new Vector2(-walletGraphic.Offset.X, walletGraphic.Offset.Y);
        }

        chairControl.MouseEntered += OnMouseEntered;
        chairControl.MouseExited += OnMouseExited;
        chairControl.GuiInput += OnGuiInput;
    }

    public bool CanHold(Draggable draggable) => draggable is DraggableCustomer && IsEmpty;

    public void AttachCustomer(ACustomer customer)
    {
        if (!IsEmpty)
        {
            GD.PushError("[Chair]: Double attach!");
            return;
        }
        Customer = customer;
        CustomerSitting = true;
        chairControl.ShaderModulate = customer.Color;
        chairControl.MouseDefaultCursorShape = Control.CursorShape.PointingHand;
        customer.SitDown(this);
        EmitSignal(SignalName.OnCustomerAttached, customer);
    }

    public void DetachCustomer()
    {
        if (IsEmpty)
        {
            GD.PushError("[Chair]: Empty detach!");
            return;
        }
        EmitSignal(SignalName.OnCustomerDetached, Customer);
        Customer = null;
        chairControl.ShaderModulate = Colors.White;
        chairControl.MouseDefaultCursorShape = Control.CursorShape.Arrow;
    }

    public void CustomerStartBreak()
    {
        if (IsEmpty)
        {
            GD.PushError("[Chair]: Empty break!");
            return;
        }
        CustomerSitting = false;
        walletGraphic.Visible = Customer.Inventory.Cards.Count > 0;
        PauseGame();
    }

    public void CustomerEndBreak()
    {
        if (IsEmpty)
        {
            GD.PushError("[Chair]: Empty end break!");
            return;
        }
        CustomerSitting = true;
        walletGraphic.Visible = false;
        ResumeGame();
    }

    public void PauseGame() => EmitSignal(SignalName.OnCustomerPaused, Customer);

    public void ResumeGame() => EmitSignal(SignalName.OnCustomerResumed, Customer);

    protected virtual void OnMouseEntered()
    {
        if (!IsEmpty)
        {
            if (CustomerSitting)
            {
                UITooltipController.Current.ShowTooltip(this, "Trade", true);
            }
            else if (Customer.Inventory.Cards.Count > 0)
            {
                UITooltipController.Current.ShowTooltip(this, "Rob", true);
            }
        }
    }

    protected virtual void OnMouseExited()
    {
        if (!IsEmpty)
        {
            UITooltipController.Current.HideTooltip(this);
        }
    }

    protected virtual void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && !mouseEvent.IsEcho())
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left || mouseEvent.ButtonIndex == MouseButton.Middle)
            {
                EmitSignal(SignalName.OnLeftClick, this);
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                EmitSignal(SignalName.OnRightClick, this);
            }
        }
    }
}
