using Godot;
using System;
using System.Collections.Generic;

public partial class CardsLoader : Node
{
    private static CardsLoader Instance;

    [Export] private Texture2D missingArt;

    private List<CardData> cards = new List<CardData>()
    {
        new CardData(
            "HAMSTR/X",
            Rarity.Common,
            "HAMSTRIX",
            "Sorcery",
            "Summon three 3/1 Hamster tokens.",
            5
        )
    };

    public override void _Ready()
    {
        base._Ready();
        if (Instance != null)
        {
            QueueFree();
            return;
        }
        Instance = this;

        cards.ForEach(a => a.Art = ResourceLoader.Load<Texture2D>("@res://Sprites/CardArt/" + a.ArtPath) ?? missingArt);
    }

    public static CardData GetCard(int id) => Instance.cards[id];
}
