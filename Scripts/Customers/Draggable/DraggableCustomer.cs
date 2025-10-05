using Godot;
using System;

public partial class DraggableCustomer : Draggable
{
    public ACustomer Customer { get; private set; }

    public void Init(ACustomer customer)
    {
        Customer = customer;
        InitMaterial(customer.Material);
    }
}
