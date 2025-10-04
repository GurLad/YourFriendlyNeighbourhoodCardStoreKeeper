using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryRendererTrade : AInventoryRenderer
{
    [Export] private GridContainer cardsGrid;

    protected override List<AInventoryCard> Filter(InventoryData data) => data.Cards.FindAll(a => a is InventoryIDCard);

    protected override void Render(List<AInventoryCard> datas)
    {
        foreach (InventoryIDCard data in datas.ConvertAll(a => (InventoryIDCard)a))
        {
            InventoryCardRenderer renderer = RenderItem(data.Data);
            cardsGrid.AddChild(renderer);
        }
    }

    protected override string InitButton(InventoryCardRenderer renderer)
    {
        throw new NotImplementedException();
    }
}
