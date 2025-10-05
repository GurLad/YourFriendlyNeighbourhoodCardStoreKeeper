using Godot;
using System;

public partial class CustomerLeech : ACustomer
{
    protected override Vector2 queueWaitTimeRange => new Vector2(30, 45);
    protected override Vector2 playTimeRange => new Vector2(9999, 9999);
    protected override Vector2 ratingRange => new Vector2(650, 1100);
    protected override Vector2 bladderRange => new Vector2(20, 25);
    protected override Vector2 toiletTimeRange => new Vector2(7, 11);

    protected override float ratingVariance => 100;
}
