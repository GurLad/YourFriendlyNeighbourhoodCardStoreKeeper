using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryRendererTrade : AInventoryRenderer
{
    private InventoryData inventory;

    public override void Render(InventoryData inventory)
    {
        this.inventory = inventory;
        foreach (InventoryIDCard data in inventory.Cards.FindAll(a => a is InventoryIDCard).ConvertAll(a => (InventoryIDCard)a))
        {
            InventoryCardRenderer renderer = RenderItem(data);
        }
    }

    protected override void InitButton(InventoryCardRenderer renderer)
    {
        renderer.InitButton(
            "Buy (" + renderer.Card.Data.Price + "$)",
            () => MoneyController.CurrentAmount >= renderer.Card.Data.Price,
            () =>
            {
                inventory.Cards.Remove(renderer.Card);
                gridContainer.RemoveItem(renderer);
                MoneyController.SpendMoney(renderer.Card.Data.Price);
                PlayerInventoryController.AddCard(renderer.Card);
            }
        );
    }
}
