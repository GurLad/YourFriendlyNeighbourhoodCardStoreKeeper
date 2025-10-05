using Godot;
using System;

public partial class ChairControl : ADropPoint
{
    public Chair Chair { private get; set; }

    public override void Drop(Draggable draggable)
    {
        if (draggable is DraggableCustomer draggableCustomer)
        {
            Chair.AttachCustomer(draggableCustomer.Customer);
        }
        else
        {
            GD.PushError("[ChairControl]: Bad drop!");
        }
    }

    protected override bool CanDrop(Draggable draggable) => Chair.CanHold(draggable);
}
