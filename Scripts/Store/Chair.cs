using Godot;
using System;

public partial class Chair : Sprite2D
{
    [Export] public Vector2 PosMod { get; set; } = new Vector2(5, 5);
    [Export] private ChairControl chairControl;

    public bool IsEmpty => customer == null;

    private ACustomer customer = null;

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
        this.customer = customer;
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
        EmitSignal(SignalName.OnCustomerDetached, customer);
        customer = null;
        chairControl.ShaderModulate = Colors.White;
    }

    public void PauseGame() => EmitSignal(SignalName.OnCustomerPaused, customer);
    
    public void ResumeGame() => EmitSignal(SignalName.OnCustomerResumed, customer);
}
