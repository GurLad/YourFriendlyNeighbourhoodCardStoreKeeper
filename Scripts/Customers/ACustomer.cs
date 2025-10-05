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
        Sitting,
        Playing,
        Trading,
        Toilet,
        Crying,
        Fighting,
        Leaving,
        Pathing = 1 << 8,
    }

    [ExportCategory("Nodes")]
    [Export] private DraggableCustomer dragger { get; set; }

    [ExportCategory("Vars")]
    [Export] public Color Color { get; set; } = Colors.White;
    [Export] private float fadeTime { get; set; } = 0.2f;
    [Export] private float speed { get; set; } = 5f;

    public Chair Chair { get; set; } = null;
    public StoreQueue Queue { private get; set; } = null;
    public InventoryData Inventory { get; } = new InventoryData();

    protected abstract Vector2 queueWaitTimeRange { get; }
    protected abstract Vector2 playTimeRange { get; }
    protected abstract Vector2 ratingRange { get; }
    protected abstract Vector2 bladderRange { get; }
    protected abstract Vector2 toiletTimeRange { get; }
    protected abstract Vector2I cardCountRange { get; }

    protected abstract float ratingVariance { get; }

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
    private float bladder { get; set; }
    private float toiletTime { get; set; }
    private int rating { get; set; }

    private Timer queueTimer { get; } = new Timer();
    private Timer playTimer { get; } = new Timer();
    private Timer bladderTimer { get; } = new Timer();
    private Timer toiletTimer { get; } = new Timer();

    private Interpolator interpolator { get; } = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        AddChild(queueTimer);
        AddChild(playTimer);
        AddChild(bladderTimer);
        AddChild(toiletTimer);

        queueTimer.WaitTime = queueWaitTime = queueWaitTimeRange.RandomValueInRange();
        playTimer.WaitTime = playTime = playTimeRange.RandomValueInRange();
        bladderTimer.WaitTime = bladder = bladderRange.RandomValueInRange();
        toiletTimer.WaitTime = toiletTime = toiletTimeRange.RandomValueInRange();
        queueTimer.OneShot = playTimer.OneShot = bladderTimer.OneShot = toiletTimer.OneShot = false;
        // Normal dist. is too much for now
        rating = Mathf.RoundToInt((ratingRange.RandomValueInRange() + ratingRange.RandomValueInRange()) / 2);

        int inventoryCount = cardCountRange.RandomValueInRange();
        for (int i = 0; i < inventoryCount; i++)
        {
            Inventory.Cards.Add(new InventoryIDCard(CardsLoader.RandomCard().ID));
        }

        AddChild(interpolator);
        interpolator.InterruptMode = Interpolator.Mode.Error;

        dragger.Init(this);
        dragger.CanDrag = false;
        dragger.MouseEntered += OnMouseEntered;
        dragger.MouseExited += OnMouseExited;
        dragger.OnPickedUp += OnPickedUp;
        dragger.OnDropped += OnDropped;
        dragger.OnCancelled += OnCancelled;

        queueTimer.Timeout += OnQueueTimerOver;
        playTimer.Timeout += OnPlayTimerOver;
        bladderTimer.Timeout += OnBladderTimerOver;
        toiletTimer.Timeout += OnToiletTimerOver;

        Position = PathExtensions.ENTRANCE_POS.ToPos();
        Scale = Vector2.Zero;

        // Blergh
        Stats.OnTheftDetected += LeaveStore;

        SoundController.Current.PlaySFX("Enter", false);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Stats.OnTheftDetected -= LeaveStore;
    }

    public void Spawn()
    {
        if (fullState != State.None)
        {
            GD.PushError("[Customer AI]: Spawning during another state!");
        }
        fullState = State.Spawning | State.Pathing;
        interpolator.Interpolate(fadeTime,
            new Interpolator.InterpolateObject(
                a => Scale = Vector2.One * a,
                Scale.X,
                1
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
        dragger.CanDrag = true;
        queueTimer.Start();
    }

    public void UpdateQueuePos(Vector2I newPos)
    {
        if (newPos.ToPos().DistanceTo(Position) <= 0.01f)
        {
            return;
        }
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
        interpolator.InterpolateMoveOnPath(this, speed, Position, newPos.ToPos());
        interpolator.OnFinish = () => pathing = false;
    }

    public void LeaveStore()
    {
        if (state == State.Toilet && !pathing)
        {
            // Miss the commotion
            return;
        }
        if (pathing)
        {
            interpolator.Stop(false);
        }
        if (Queue != null && state == State.Queue)
        {
            Queue.RemoveCustomer(this);
        }
        playTimer.Stop();
        queueTimer.Stop();
        bladderTimer.Stop();
        toiletTimer.Stop();
        fullState = State.Leaving | State.Pathing;
        if (Chair != null)
        {
            Chair.DetachCustomer();
        }
        interpolator.InterpolateMoveOnPath(this, speed, Position, PathExtensions.ENTRANCE_POS.ToPos());
        interpolator.OnFinish = () =>
        {
            interpolator.Interpolate(fadeTime,
                new Interpolator.InterpolateObject(
                    a => Scale = Vector2.One * a,
                    Scale.X,
                    0
                ));
            interpolator.OnFinish = QueueFree;
        };
    }

    public void SitDown(Chair chair)
    {
        if (state != State.Queue)
        {
            GD.PushError("[Customer AI]: SitDown when not in queue!");
        }
        if (pathing)
        {
            interpolator.Stop(false);
        }
        dragger.CanDrag = false;
        dragger.Visible = false;
        dragger.RenderHighlight(true);
        Queue.RemoveCustomer(this);
        pathing = false;
        fullState = State.Sitting;
        Chair = chair;
        GlobalPosition = chair.GlobalPosition + chair.PosMod;
        RotationDegrees = chair.FlipH ? 0 : 180;
        queueTimer.Stop();
        bladderTimer.Start();
        playTimer.Start();
    }

    public void TakeBreak()
    {
        if (state != State.Playing && state != State.Sitting)
        {
            GD.PushError("[Customer AI]: TakeBreak when not sitting/playing!");
        }
        if (!playTimer.Paused)
        {
            playTimer.Paused = true;
        }
        else
        {
            GD.PushWarning("[ACustomer]: Messed up play timer?");
        }
        fullState = State.Toilet | State.Pathing;
        interpolator.InterpolateMoveOnPath(this, speed, Position, PathExtensions.TOILET_POS.ToPos());
        interpolator.OnFinish = () =>
        {
            pathing = false;
            interpolator.Interpolate(fadeTime,
                new Interpolator.InterpolateObject(
                    a => Scale = Vector2.One * a,
                    Scale.X,
                    0
                ));
            interpolator.OnFinish = () => toiletTimer.Start();
        };
    }

    private void ReturnFromBreak()
    {
        if (state != State.Toilet)
        {
            GD.PushError("[Customer AI]: ReturnFromBreak when not toilet!");
        }
        if (Chair == null || Chair.Customer != this)
        {
            GD.PushError("[Customer AI]: Return from break no chair!");
        }
        interpolator.Interpolate(fadeTime,
            new Interpolator.InterpolateObject(
                a => Scale = Vector2.One * a,
                Scale.X,
                1
            ));
        interpolator.OnFinish = () =>
        {
            pathing = true;
            interpolator.InterpolateMoveOnPath(this, speed, Position, Chair.GlobalPosition);
            interpolator.OnFinish = () =>
            {
                ResumeSitting();
                Chair.CustomerEndBreak();
            };
        };
    }

    public void ResumeSitting()
    {
        if (state != State.Toilet)
        {
            GD.PushError("[Customer AI]: ResumeSitting when not toilet!");
        }
        if (Chair == null || Chair.Customer != this)
        {
            GD.PushError("[Customer AI]: Return from break no chair!");
        }
        fullState = State.Sitting;
        GlobalPosition = Chair.GlobalPosition + Chair.PosMod;
        RotationDegrees = Chair.FlipH ? 0 : 180;
        bladderTimer.Start();
        if (playTimer.Paused)
        {
            playTimer.Paused = false;
        }
        else
        {
            GD.PushWarning("[ACustomer]: Messed up play timer?");
        }
    }

    public void StartResumePlaying()
    {
        if (state != State.Sitting && state != State.Trading)
        {
            GD.PushError("[Customer AI]: StartPlaying when not sitting/trading!");
        }
        fullState = State.Playing;
    }

    public void PrepareForTrade()
    {
        if (state == State.Trading)
        {
            GD.PushWarning("[Customer AI]: Double clicks suck but whatevs");
        }
        if (state != State.Sitting && state != State.Playing)
        {
            GD.PushError("[Customer AI]: PrepareForTrade when not sitting/Playing!");
        }
        if (!bladderTimer.Paused)
        {
            bladderTimer.Paused = true;
        }
        if (!playTimer.Paused)
        {
            playTimer.Paused = true;
        }
        fullState = State.Trading;
        //Chair.CustomerStartBreak();
    }

    public void FinishTrade()
    {
        if (state != State.Trading)
        {
            GD.PushError("[Customer AI]: FinishTrade when not trading! " + state);
        }
        if (bladderTimer.Paused)
        {
            bladderTimer.Paused = false;
        }
        if (playTimer.Paused)
        {
            playTimer.Paused = false;
        }
        fullState = State.Sitting;
        //Chair.CustomerEndBreak();
    }

    public virtual bool CanSeeTheft(Player player, ACustomer stealingFrom)
    {
        if (state == State.Sitting || state == State.Playing) // Should be impossible for playing but whatevs
        {
            // It's the other one
            if (stealingFrom != this)
            {
                return true;
            }
            else
            {
                GD.PushError("[ACustomer]: Stealing from a sitting/playing player!");
            }
        }
        else if (state == State.Toilet && pathing && stealingFrom == this)
        {
            return Pathfinder.HasLineOfSight(GlobalPosition.ToTile(), player.GlobalPosition.ToTile());
        }
        return false;
    }

    public virtual float GetWinPerformance() => rating + ExtensionMethods.RNG.NextFloat(-1f, 1f) * ratingVariance;

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
        queueTimer.Stop();
        LeaveStore();
    }

    private void OnPlayTimerOver()
    {
        if (state != State.Playing && state != State.Sitting)
        {
            GD.PushError("[Customer AI]: OnQueueTimerOver when not playing!");
        }
        if (Chair == null)
        {
            GD.PushError("[Customer AI]: OnQueueTimerOver when not sitting!");
        }
        playTimer.Stop();
        LeaveStore();
    }

    private void OnBladderTimerOver()
    {
        if (state != State.Playing && state != State.Sitting)
        {
            GD.PushError("[Customer AI]: OnBladderTimerOver when not playing!");
        }
        if (Chair == null)
        {
            GD.PushError("[Customer AI]: OnBladderTimerOver when not chair!");
        }
        bladderTimer.Stop();
        Chair.CustomerStartBreak();
        TakeBreak();
    }

    private void OnToiletTimerOver()
    {
        if (state != State.Toilet)
        {
            GD.PushError("[Customer AI]: OnToiletTimerOver when not toilet!");
        }
        if (Chair == null)
        {
            GD.PushError("[Customer AI]: OnToiletTimerOver when not chair!");
        }
        toiletTimer.Stop();
        ReturnFromBreak();
    }

    protected virtual void OnMouseEntered()
    {
        if (state == State.Queue)
        {
            UITooltipController.Current.ShowTooltip(this, "Rating: " + rating, true);
        }
    }

    protected virtual void OnMouseExited()
    {
        UITooltipController.Current.HideTooltip(this);
    }

    private void OnPickedUp() { }
    private void OnDropped() { }
    private void OnCancelled() { }
}
