using Godot;
using System;
using System.Collections.Generic;

public abstract partial class AInventoryRenderer : Control
{
    [Export] private PackedScene sceneCardRenderer;

    private List<InventoryCardRenderer> renderers = new List<InventoryCardRenderer>();

    protected InventoryCardRenderer RenderItem(CardData data, bool isNew = false)
    {
        InventoryCardRenderer renderer = sceneCardRenderer.Instantiate<InventoryCardRenderer>();
        renderer.Render(data, isNew);
        InitButton(renderer);
        renderer.OnButtonPressed += OnButtonPressed;
        renderers.Add(renderer);
        return renderer;
    }

    protected virtual void OnButtonPressed()
    {
        renderers.ForEach(a => a.UpdateCanPress());
    }

    protected abstract void Render(List<AInventoryCard> datas);

    protected abstract string InitButton(InventoryCardRenderer renderer);
}
