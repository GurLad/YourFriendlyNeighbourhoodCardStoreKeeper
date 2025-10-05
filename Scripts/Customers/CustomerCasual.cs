using Godot;
using System;

public partial class CustomerCasual : ACustomer
{
    protected override Vector2 queueWaitTimeRange => new Vector2(10, 15);
    protected override Vector2 playTimeRange => new Vector2(25, 35);
    protected override Vector2 ratingRange => new Vector2(400, 900);
    protected override Vector2 bladderRange => new Vector2(12, 20);
    protected override Vector2 toiletTimeRange => new Vector2(2, 4);

    protected override Vector2I cardCountRange => new Vector2I(1, 3);

    protected override float ratingVariance => 150;
}
