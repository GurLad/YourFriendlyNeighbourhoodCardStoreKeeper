using Godot;
using System;

public class InventoryIDCard : AInventoryCard
{
    public int ID { get; init; }

    public override CardData Data => CardsLoader.GetCard(ID);

    protected override bool EqualInternal(AInventoryCard other)
    {
        if (other is InventoryIDCard inventoryIDCard)
        {
            return inventoryIDCard.ID == ID;
        }
        return false;
    }
}
