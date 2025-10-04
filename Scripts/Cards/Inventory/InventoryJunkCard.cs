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
        data.Junk = true;
        data.Price = 1;
        while (ExtensionMethods.RNG.NextDouble() < (int)data.Rarity * 0.1f && data.Price < 4096)
        {
            data.Price *= 2;
        }
        if (data.Foil)
        {
            data.Price *= 2;
        }
        data.Price = Mathf.RoundToInt(Mathf.Clamp(Data.Price * (ExtensionMethods.RNG.NextDouble() + 0.5f), 1, 9999));
    }

    protected override bool EqualInternal(AInventoryCard other)
    {
        return Equals(other);
    }
}
