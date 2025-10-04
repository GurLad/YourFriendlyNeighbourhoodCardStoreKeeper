using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryRendererCollection : AInventoryRenderer
{
    private InventoryData inventory;

    public override void Render(InventoryData inventory)
    {
        this.inventory = inventory;
        List<InventoryIDCard> relevant = inventory.Cards.FindAll(a => a is InventoryIDCard).ConvertAll(a => (InventoryIDCard)a);
        List<CardData> allCards = CardsLoader.GetAllCards();
        for (int i = 0; i < allCards.Count; i++)
        {
            CardData renderData = null;
            List<InventoryIDCard> matching = relevant.FindAll(a => a.ID == i);
            if (matching.Count > 0)
            {
                InventoryIDCard best = matching.Find(a => a.Foil) ?? matching[0];
                renderData = best.Data.Clone();
                renderData.Price = matching.Count > 1 ? matching.FindAll(a => a != best).Sum(a => a.Data.Price) : 0;
            }
            else
            {
                renderData = allCards[i].Clone();
                renderData.Price = 0;
            }
            RenderItem(renderData, inventory.IsNewCard(i));
        }
    }

    protected override void InitButton(InventoryCardRenderer renderer)
    {
        renderer.InitButton(
            renderer.Data.Price > 0 ? "Sell Duplicates (" + renderer.Data.Price + "$)" : "No Duplicates",
            () => renderer.Data.Price > 0,
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
                });
            }
        );
    }
}
