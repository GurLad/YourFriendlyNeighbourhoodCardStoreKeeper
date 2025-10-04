using Godot;
using System;

public class InventoryIDCard : AInventoryCard
{
    public int ID { get; init; }
    public bool Foil { get; init; }

    private CardData data { get; init; }
    public override CardData Data => CardsLoader.GetCard(ID);

    public InventoryIDCard(int id, bool foil)
    {
        ID = id;
        Foil = foil;
        data = CardsLoader.GetCard(ID).Clone();
        if (foil)
        {
            data.Foil = true;
            data.Price = -1;
        }
    }
}
