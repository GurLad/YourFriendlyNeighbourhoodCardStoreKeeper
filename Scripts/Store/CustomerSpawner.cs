using Godot;
using System;

public partial class CustomerSpawner : Node
{
    [Export] private StoreQueue queue;
    [Export] private PackedScene[] customerScenes;
    [Export] private Node customerHolder;
    [Export] private Vector2 rate;

    private Timer spawnTimer { get; } = new Timer();

    public override void _Ready()
    {
        base._Ready();
        AddChild(spawnTimer);
        spawnTimer.WaitTime = rate.RandomValueInRange();
        spawnTimer.OneShot = false;
        spawnTimer.Timeout += TrySpawn;
        spawnTimer.Start();
    }

    private void TrySpawn()
    {
        spawnTimer.WaitTime = rate.RandomValueInRange();
        if (queue.Full)
        {
            return;
        }
        ACustomer newCustomer = customerScenes.RandomItemInList().Instantiate<ACustomer>();
        customerHolder.AddChild(newCustomer);
        newCustomer.Spawn();
        queue.InsertCustomer(newCustomer);
    }
}
