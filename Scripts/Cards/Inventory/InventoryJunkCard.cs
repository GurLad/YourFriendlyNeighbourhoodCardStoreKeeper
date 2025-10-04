using Godot;
using System;

public class InventoryJunkCard : AInventoryCard
{
    private CardData data { get; init; }
    public override CardData Data => throw new NotImplementedException();

    public InventoryJunkCard(Rarity? rarity = null, bool? foil = null)
    {
        data = new CardData(
            "TBA",
            rarity ?? (Rarity)ExtensionMethods.RNG.Next(0, (int)Rarity.End),
            "",
            "TBA",
            "TBA",
            ExtensionMethods.RNG.Next(0, 10),
            ExtensionMethods.RNG.Next(-1, 10),
            ExtensionMethods.RNG.Next(-1, 10),
            foil ?? ExtensionMethods.RNG.NextDouble() <= 0.125f
        );
    }

    protected override bool EqualInternal(AInventoryCard other)
    {
        return Equals(other);
    }
}
