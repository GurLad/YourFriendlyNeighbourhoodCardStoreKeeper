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
            "Fluffy Pirate",
            Rarity.Common,
            "FluffyPirate",
            "Summon Hamster",
            "\a\"Yaaaaaar! I'm a-pirate!\"",
            2,
            2,
            2
        ),
        new CardData(
            "HAMSTR/X",
            Rarity.Uncommon,
            "HAMSTRIX",
            "Sorcery",
            "Summon three 3/1 Hamster tokens.",
            5
        ),
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

        cards.ForEach(a => a.Art = GetArt(a.ArtPath));
    }

    public static CardData GetCard(int id) => Instance.cards[id];

    public static Texture2D GetArt(string artPath) => ResourceLoader.Load<Texture2D>("@res://Sprites/CardArt/" + artPath) ?? Instance.missingArt;
}
