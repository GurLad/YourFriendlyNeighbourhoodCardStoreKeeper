using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryRendererJunk : AInventoryRenderer
{
    private InventoryData inventory;

    public override void Render(InventoryData inventory)
    {
        this.inventory = inventory;
        foreach (InventoryIDCard data in inventory.Cards.FindAll(a => a is InventoryJunkCard).ConvertAll(a => (InventoryIDCard)a))
        {
            InventoryCardRenderer renderer = RenderItem(data);
        }
    }

    protected override void InitButton(InventoryCardRenderer renderer)
    {
        renderer.InitButton(
            "Sell (" + renderer.Card.Data.Price + "$)",
            () => true,
            () =>
            {
                PlayerInventoryController.TrySell(() =>
                {
                    inventory.Cards.Remove(renderer.Card);
                    gridContainer.RemoveItem(renderer);
                    MoneyController.GainMoney(renderer.Card.Data.Price);
                    OnButtonPressed();
                });
            }
        );
    }
}
