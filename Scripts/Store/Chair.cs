using Godot;
using System;

public partial class Chair : Sprite2D
{
    public bool IsEmpty => customer == null;

    private ACustomer customer = null;

    public void AttachCustomer(ACustomer customer)
    {
        if (!IsEmpty)
        {
            GD.PushError("[Chair]: Double attach!");
            return;
        }
        this.customer = customer;
        customer.Chair = this;
        Modulate = customer.Color;
    }

    public void DetachCustomer()
    {
        if (IsEmpty)
        {
            GD.PushError("[Chair]: Empty detach!");
            return;
        }
        customer = null;
        Modulate = Colors.White;
    }
}
