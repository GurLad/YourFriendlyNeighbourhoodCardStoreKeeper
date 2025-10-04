using Godot;
using System;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    End
}

public class CardData
{
    public string Name { get; init; }
    public Rarity Rarity { get; init; }
    public string ArtPath { get; init; }
    public string Type { get; init; }
    public string Desc { get; init; }
    public int Cost { get; init; }
    public int Power { get; init; } = -1;
    public int Toughness { get; init; } = -1;

    public bool Foil { get; set; } = false;
    public bool Junk { get; set; } = false;

    private Texture2D art { get; set; }
    public Texture2D Art
    {
        get => art ??= CardsLoader.GetArt(ArtPath);
        set => art = value;
    }

    public CardData(string name, Rarity rarity, string artPath, string type, string desc, int cost)
    {
        Name = name;
        Rarity = rarity;
        ArtPath = artPath;
        Type = type;
        Desc = desc;
        Cost = cost;
    }

    public CardData(string name, Rarity rarity, string artPath, string type, string desc, int cost, int power, int toughness)
    {
        Name = name;
        Rarity = rarity;
        ArtPath = artPath;
        Type = type;
        Desc = desc;
        Cost = cost;
        Power = power;
        Toughness = toughness;
    }

    public CardData(string name, Rarity rarity, string artPath, string type, string desc, int cost, int power, int toughness, bool foil)
    {
        Name = name;
        Rarity = rarity;
        ArtPath = artPath;
        Type = type;
        Desc = desc;
        Cost = cost;
        Power = power;
        Toughness = toughness;
        Foil = foil;
    }
}
