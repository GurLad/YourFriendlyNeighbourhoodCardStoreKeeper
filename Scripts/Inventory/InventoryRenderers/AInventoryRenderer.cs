using Godot;
using System;
using System.Collections.Generic;

public abstract partial class AInventoryRenderer : Control
{
    [Export] private PackedScene sceneCardRenderer;

    [ExportCategory("Interploator vars")]
    [Export] private float ShowHideTime = 0.3f;

    private List<InventoryCardRenderer> renderers = new List<InventoryCardRenderer>();
    private Interpolator interpolator { get; } = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
    }

    public void AnimateShow()
    {
        TotalMouseBlock.Block();
        StoreController.Pause();
        interpolator.Interpolate(ShowHideTime,
            new Interpolator.InterpolateObject(
                a => Modulate = a,
                Modulate,
                Colors.White,
                Easing.EaseInOutSin
            ));
        interpolator.OnFinish = TotalMouseBlock.Unblock;
    }

    public void AnimateHide()
    {
        TotalMouseBlock.Block();
        StoreController.Pause();
        interpolator.Interpolate(ShowHideTime,
            new Interpolator.InterpolateObject(
                a => Modulate = a,
                Modulate,
                Colors.Transparent,
                Easing.EaseInOutSin
            ));
        interpolator.OnFinish = TotalMouseBlock.Unblock;
    }

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
