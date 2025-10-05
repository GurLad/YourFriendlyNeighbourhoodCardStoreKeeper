using Godot;
using System;
using System.Collections.Generic;

public abstract partial class ACustomer : Sprite2D
{
    private enum State
    {
        None,
        Spawning,
        Queue,
        Playing,
        Toilet,
        Crying,
        Fighting,
        Leaving,
        Pathing = 1 << 8,
    }

    [Export] public Color Color { get; set; } = Colors.White;
    [Export] private float fadeTime { get; set; } = 0.2f;
    [Export] private float speed { get; set; } = 5f;

    public Chair Chair { private get; set; } = null;
    public StoreQueue Queue { private get; set; } = null;

    protected abstract Vector2 queueWaitTimeRange { get; }
    protected abstract Vector2 playTimeRange { get; }
    protected abstract Vector2 ratingRange { get; }
    protected abstract Vector2 bladderRange { get; }
    protected abstract Vector2 toiletTimeRange { get; }

    private List<Action> actionQueue { get; } = new List<Action>();
    private State fullState { get; set; } = State.None;
    private State state => fullState & ~State.Pathing;
    private bool pathing
    {
        get => (fullState & State.Pathing) != State.None;
        set => fullState = value ? (fullState | State.Pathing) : (fullState & ~State.Pathing);
    }

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

    public void Spawn()
    {
        if (fullState != State.None)
        {
            GD.PushError("[Customer AI]: Spawning during another state!");
        }
        fullState = State.Spawning | State.Pathing;
        interpolator.Interpolate(fadeTime,
            Interpolator.InterpolateObject.ModulateFadeInterpolate(
                    this,
                    Colors.White
                ));
        interpolator.OnFinish = () =>
        {
            pathing = false;
            TryActionFromQueue();
        };
    }

    public void EnterQueue()
    {
        if (state != State.Spawning)
        {
            GD.PushError("[Customer AI]: EnterQueue when not spawning!");
        }
        if (pathing)
        {
            actionQueue.Add(EnterQueue);
            return;
        }
        fullState = State.Queue;
        queueTimer.Start();
    }

    public void UpdateQueuePos(Vector2I newPos)
    {
        if (state == State.Spawning)
        {
            actionQueue.Add(() => UpdateQueuePos(newPos));
            return;
        }
        if (state != State.Queue)
        {
            GD.PushError("[Customer AI]: UpdateQueuePos when not in queue!");
        }
        if (pathing)
        {
            interpolator.Stop(false);
        }
        pathing = true;
        interpolator.InterpolateMoveOnPath(this, speed, Position, newPos);
        interpolator.OnFinish = () => pathing = false;
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
            interpolator.Interpolate(fadeTime,
                Interpolator.InterpolateObject.ModulateFadeInterpolate(
                    this,
                    Colors.Transparent
                ));
            interpolator.OnFinish = QueueFree;
        };
    }

    private void TryActionFromQueue()
    {
        while (!pathing && actionQueue.Count > 0)
        {
            actionQueue[0].Invoke();
            actionQueue.RemoveAt(0);
        }
    }

    private void OnQueueTimerOver()
    {
        if (state != State.Queue)
        {
            GD.PushError("[Customer AI]: OnQueueTimerOver when not in queue!");
        }
        if (Chair != null)
        {
            GD.PushError("[Customer AI]: OnQueueTimerOver when sitting!");
        }
        LeaveStore();
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
