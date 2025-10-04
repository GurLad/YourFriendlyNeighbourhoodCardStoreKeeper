using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryRendererJunk : AInventoryRenderer
{
    protected override List<AInventoryCard> Filter(InventoryData data) => data.Cards.FindAll(a => a is InventoryJunkCard);

    protected override void Render(List<AInventoryCard> datas)
    {
        throw new NotImplementedException();
    }

    protected override string InitButton(InventoryCardRenderer renderer)
    {
        throw new NotImplementedException();
    }
}
