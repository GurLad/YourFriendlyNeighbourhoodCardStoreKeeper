using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Table : Node2D
{
    [Export] private Sprite2D sprite;
    [Export] public Chair[] chairs;
    [ExportCategory("Vars")]
    [Export] private Texture2D emptyTable;
    [Export] private Texture2D gameTable;

    private List<ACustomer> playingCustomers { get; } = new List<ACustomer>();

    public override void _Ready()
    {
        base._Ready();
        chairs.ToList().ForEach(a =>
        {
            a.OnCustomerAttached += OnCustomerAttached;
            a.OnCustomerDetached += OnCustomerDetached;
            a.OnCustomerPaused += OnPaused;
            a.OnCustomerResumed += OnResumed;
        });
    }

    public void OnCustomerAttached(ACustomer customer)
    {
        playingCustomers.Add(customer);
        if (playingCustomers.Count >= chairs.Length)
        {
            sprite.Texture = gameTable;
        }
    }

    public void OnCustomerDetached(ACustomer customer)
    {
        playingCustomers.Remove(customer);
        sprite.Texture = emptyTable;
    }

    public void OnPaused(ACustomer customer)
    {
        // TBA playing stuff...
    }

    public void OnResumed(ACustomer customer)
    {
        // TBA playing stuff...
    }
}
