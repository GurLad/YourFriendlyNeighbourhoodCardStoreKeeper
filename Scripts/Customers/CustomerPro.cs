using Godot;
using System;

public partial class CustomerPro : ACustomer
{
    protected override Vector2 queueWaitTimeRange => new Vector2(7, 12);
    protected override Vector2 playTimeRange => new Vector2(60, 85);
    protected override Vector2 ratingRange => new Vector2(800, 1600);
    protected override Vector2 bladderRange => new Vector2(16, 25);
    protected override Vector2 toiletTimeRange => new Vector2(3, 7);

    protected override Vector2I cardCountRange => new Vector2I(5, 8);

    protected override float ratingVariance => 200;
}
