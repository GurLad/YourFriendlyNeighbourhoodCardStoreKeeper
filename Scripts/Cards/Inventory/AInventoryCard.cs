using Godot;
using System;

public abstract class AInventoryCard
{
    public abstract CardData Data { get; }

    protected abstract bool EqualInternal(AInventoryCard other);

    public static bool operator ==(AInventoryCard a, AInventoryCard b)
    {
        if ((object)a == null) return (object)b == null;
        if ((object)b == null) return (object)a == null;
        return a.EqualInternal(b);
    }

    public static bool operator !=(AInventoryCard a, AInventoryCard b)
    {
        return !(a == b);
    }
}
