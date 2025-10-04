using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryRendererJunk : AInventoryRenderer
{
    protected override List<AInventoryCard> Filter(InventoryData data) => data.Cards.FindAll(a => a is InventoryJunkCard);

    public override void Render(InventoryData inventory)
    {
        throw new NotImplementedException();
    }

    protected override void InitButton(InventoryCardRenderer renderer)
    {
        throw new NotImplementedException();
    }
}
