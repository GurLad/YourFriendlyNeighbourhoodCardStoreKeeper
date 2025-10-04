using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryRendererCollection : AInventoryRenderer
{
    [Export] private Color lockedModulate = Colors.Black;

    private InventoryData inventory;

    public override void _Ready()
    {
        base._Ready();
        // TEMP

        Render(PlayerInventoryController.Inventory);
    }

    public override void Render(InventoryData inventory)
    {
        this.inventory = inventory;
        List<InventoryIDCard> relevant = inventory.Cards.FindAll(a => a is InventoryIDCard).ConvertAll(a => (InventoryIDCard)a);
        List<CardData> allCards = CardsLoader.GetAllCards();
        for (int i = 0; i < allCards.Count; i++)
        {
            CardData renderData = null;
            List<InventoryIDCard> matching = relevant.FindAll(a => a.ID == i);
            bool owned = false;
            if (matching.Count > 0)
            {
                InventoryIDCard best = matching.Find(a => a.Foil) ?? matching[0];
                renderData = best.Data.Clone();
                renderData.Price = matching.Count > 1 ? matching.FindAll(a => a != best).Sum(a => a.Data.Price) : 0;
                owned = true;
            }
            else
            {
                renderData = allCards[i].Clone();
                renderData.Price = 0;
            }
            InventoryCardRenderer renderer = RenderItem(renderData, inventory.IsNewCard(i));
            if (!owned)
            {
                renderer.Modulate = lockedModulate;
                renderer.HideButton();
            }
        }
    }

    protected override void InitButton(InventoryCardRenderer renderer)
    {
        renderer.InitButton(
            renderer.Data.Price > 0 ? "Sell Copies (" + renderer.Data.Price + "$)" : "No Duplicates",
            () =>
            {
                return renderer.Data.Price > 0;
            },
            () =>
            {
                PlayerInventoryController.TrySell(() =>
                {
                    List<InventoryIDCard> relevant = inventory.Cards.FindAll(a => a is InventoryIDCard).ConvertAll(a => (InventoryIDCard)a);
                    List<InventoryIDCard> matching = relevant.FindAll(a => a.ID == renderer.Data.ID);
                    InventoryIDCard best = matching.Find(a => a.Foil) ?? matching[0];
                    matching.FindAll(a => a != best).ForEach(a => inventory.Cards.Remove(a));
                    MoneyController.GainMoney(renderer.Data.Price);
                    renderer.Data.Price = 0;
                    renderer.UpdateButtonText("No Duplicates");
                    renderer.UpdateCanPress();
                });
            }
        );
    }
}
