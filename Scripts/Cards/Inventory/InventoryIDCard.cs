using Godot;
using System;

public class InventoryIDCard : AInventoryCard
{
    public int ID { get; init; }
    public bool Foil { get; init; }

    private CardData data { get; init; }
    public override CardData Data => data;

    public InventoryIDCard(int id, bool? foil = null)
    {
        ID = id;
        Foil = foil ?? ExtensionMethods.RNG.NextDouble() <= 0.125f;
        data = CardsLoader.GetCard(ID).Clone();
        if (Foil)
        {
            data.Foil = true;
            data.Price = -1;
        }
    }
}
