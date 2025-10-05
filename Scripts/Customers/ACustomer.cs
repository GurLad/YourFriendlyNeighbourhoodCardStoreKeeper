using Godot;
using System;
using System.Collections.Generic;

public abstract partial class ACustomer : Sprite2D
{
    private enum State
    {
        None,
        Queue,
        Playing,
        Toilet,
        Crying,
        Fighting,
        Leaving,
        Pathing = 1 << 8,
    }

    [Export] public Color Color { get; set; } = Colors.White;
    [Export] private float fadeOutTime { get; set; } = 0.2f;
    [Export] private float speed { get; set; } = 5f;

    public Chair Chair { private get; set; } = null;

    protected abstract Vector2 queueWaitTimeRange { get; }
    protected abstract Vector2 playTimeRange { get; }
    protected abstract Vector2 ratingRange { get; }
    protected abstract Vector2 bladderRange { get; }
    protected abstract Vector2 toiletTimeRange { get; }

    private List<Action> actionQueue { get; } = new List<Action>();
    private State fullState { get; set; } = State.Queue;
    private State state => fullState & ~State.Pathing;
    private bool pathing => (fullState & State.Pathing) != State.None;

    private float queueWaitTime { get; set; }
    private float playTime { get; set; }
    private float rating { get; set; }
    private float bladder { get; set; }
    private float toiletTime { get; set; }

    private Timer queueTimer { get; set; }
    private Timer playTimer { get; set; }
    private Timer bladderTimer { get; set; }
    private Timer toiletTimer { get; set; }

    private Interpolator interpolator { get; } = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        queueTimer.WaitTime = queueWaitTime = queueWaitTimeRange.RandomValueInRange();
        playTimer.WaitTime = playTime = playTimeRange.RandomValueInRange();
        bladderTimer.WaitTime = bladder = bladderRange.RandomValueInRange();
        toiletTimer.WaitTime = toiletTime = toiletTimeRange.RandomValueInRange();
        rating = ratingRange.RandomValueInRange();

        AddChild(interpolator);
        interpolator.InterruptMode = Interpolator.Mode.Error;
    }

    public void EnterQueue()
    {
        queueTimer.Start();
    }

    public void LeaveStore()
    {
        if (pathing)
        {
            interpolator.Stop(false);
        }
        fullState = State.Leaving | State.Pathing;
        if (Chair != null)
        {
            Chair.DetachCustomer();
        }
        interpolator.InterpolateMoveOnPath(this, speed, Position, PathExtensions.ENTRANCE_POS);
        interpolator.OnFinish = () =>
        {
            interpolator.Interpolate(fadeOutTime,
                Interpolator.InterpolateObject.ModulateFadeInterpolate(
                    this,
                    Colors.Transparent
                ));
            interpolator.OnFinish = QueueFree;
        };
    }

    private void OnQueueTimerOver()
    {
        if (state != State.Queue && state != State.Pathing)
        {
            GD.PushError("[Customer AI]: OnQueueTimerOver when not in queue!");
        }
        if (Chair != null)
        {
            GD.PushError("[Customer AI]: OnQueueTimerOver when sitting!");
        }
    }

    private void OnPlayTimerOver()
    {

    }

    private void OnBladderTimerOver()
    {

    }

    private void OnToiletTimerOver()
    {

    }
}
