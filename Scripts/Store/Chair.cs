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

    public override void _Ready()
    {
        base._Ready();
        chairControl.Chair = this;
        if (FlipH)
        {
            PosMod = new Vector2(-PosMod.X, PosMod.Y);
            walletGraphic.Offset = new Vector2(-walletGraphic.Offset.X, walletGraphic.Offset.Y);
        }
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
    }

    public void CustomerStartBreak()
    {
        if (IsEmpty)
        {
            GD.PushError("[Chair]: Empty break!");
            return;
        }
        CustomerSitting = false;
        walletGraphic.Visible = true;
        PauseGame();
        Customer.TakeBreak();
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
        Customer.ResumeSitting();
        ResumeGame();
    }

    public void PauseGame() => EmitSignal(SignalName.OnCustomerPaused, Customer);

    public void ResumeGame() => EmitSignal(SignalName.OnCustomerResumed, Customer);
}
