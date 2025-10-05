using Godot;
using System;

public partial class CustomerKid : ACustomer
{
    protected override Vector2 queueWaitTimeRange => new Vector2(4, 7);
    protected override Vector2 playTimeRange => new Vector2(5, 15);
    protected override Vector2 ratingRange => new Vector2(200, 700);
    protected override Vector2 bladderRange => new Vector2(2, 4);
    protected override Vector2 toiletTimeRange => new Vector2(1, 2);

    protected override Vector2I cardCountRange => new Vector2I(1, 8);

    protected override float ratingVariance => 1300;
}
