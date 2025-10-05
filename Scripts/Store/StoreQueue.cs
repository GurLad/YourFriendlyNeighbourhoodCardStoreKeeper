using Godot;
using System;
using System.Collections.Generic;

public partial class StoreQueue : Node
{
    [Export] private Vector2I queueStartPos = PathExtensions.QUEUE_START_POS;
    [Export] private Vector2I queueEndPos = PathExtensions.ENTRANCE_POS + Vector2I.Left;

    private float capacity => queueEndPos.X - queueStartPos.X + 1;

    public bool Full => customers.Count >= capacity;

    private List<ACustomer> customers = new List<ACustomer>();

    public void InsertCustomer(ACustomer customer)
    {
        if (Full)
        {
            GD.PushError("[StoreQueue]: Inserting in a full queue!");
            return;
        }
        customers.Add(customer);

    }
}
