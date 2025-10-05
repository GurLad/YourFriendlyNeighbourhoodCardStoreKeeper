using Godot;
using System;

public abstract partial class Customer : Node2D
{
    protected abstract Vector2 queueWaitTimeRange { get; }
    protected abstract Vector2 playTimeRange { get; }
    protected abstract Vector2 ratingRange { get; }
    protected abstract Vector2 bladderRange { get; }

    private float queueWaitTime { get; set; }
    private float playTime { get; set; }
    private float rating { get; set; }
    private float bladder { get; set; }

    public override void _Ready()
    {
        base._Ready();
        queueWaitTime = queueWaitTimeRange.RandomValueInRange();
    }
}
