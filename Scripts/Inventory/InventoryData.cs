using Godot;
using System;
using System.Collections.Generic;

public class InventoryData
{
    public List<AInventoryCard> Cards { get; } = new List<AInventoryCard>();

    private List<int> seenCards { get; } = new List<int>();

    public bool IsNewCard(int id)
    {
        if (seenCards.Contains(id)) return false;
        if (Cards.Find(a => a is InventoryIDCard idCard && idCard.ID == id) != null)
        {
            seenCards.Add(id);
            return true;
        }
        return false;
    }
}
