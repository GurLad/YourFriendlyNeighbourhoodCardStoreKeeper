using Godot;
using System;

public partial class Chair : Sprite2D
{
    [Export] public Vector2 PosMod { get; set; } = new Vector2(5, 5);
    [Export] private ChairControl chairControl;

    public bool IsEmpty => customer == null;

    private ACustomer customer = null;

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
    }

    public void DetachCustomer()
    {
        if (IsEmpty)
        {
            GD.PushError("[Chair]: Empty detach!");
            return;
        }
        customer = null;
        chairControl.ShaderModulate = Colors.White;
    }
}
