using Godot;
using System;

public partial class DraggableCustomer : Draggable
{
    public ACustomer Customer { get; private set; }

    public void Init(ACustomer customer)
    {
        Customer = customer;
        Customer.Material = InitMaterial(customer.Material);
        DragIcon = Customer.Texture;
    }
}
