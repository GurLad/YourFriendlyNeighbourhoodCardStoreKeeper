using Godot;
using System;
using System.Collections.Generic;

public abstract partial class AInventoryRenderer : Control
{
    [Export] protected MultiPageGridContainer gridContainer;
    [Export] private PackedScene sceneCardRenderer;

    [ExportCategory("Interploator vars")]
    [Export] private float ShowHideTime = 0.3f;

    private List<InventoryCardRenderer> renderers = new List<InventoryCardRenderer>();
    private Interpolator interpolator { get; } = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);

        Modulate = Colors.Transparent;
    }

    public void AnimateShow()
    {
        Visible = true;
        TotalMouseBlock.Block();
        StoreController.Pause();
        interpolator.Interpolate(ShowHideTime,
            Interpolator.InterpolateObject.ModulateFadeInterpolate(
                this,
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
            Interpolator.InterpolateObject.ModulateFadeInterpolate(
                this,
                Colors.Transparent,
                Easing.EaseInOutSin
            ));
        interpolator.OnFinish = () => { TotalMouseBlock.Unblock(); Visible = false; };
    }

    protected InventoryCardRenderer RenderItem(CardData card, bool isNew = false)
    {
        InventoryCardRenderer renderer = sceneCardRenderer.Instantiate<InventoryCardRenderer>();
        renderer.Render(card, isNew);
        InitButton(renderer);
        renderer.OnButtonPressed += OnButtonPressed;
        renderers.Add(renderer);
        gridContainer.AddItem(renderer);
        return renderer;
    }

    protected InventoryCardRenderer RenderItem(AInventoryCard card, bool isNew = false)
    {
        InventoryCardRenderer renderer = sceneCardRenderer.Instantiate<InventoryCardRenderer>();
        renderer.Render(card, isNew);
        InitButton(renderer);
        renderer.OnButtonPressed += OnButtonPressed;
        renderers.Add(renderer);
        gridContainer.AddItem(renderer);
        return renderer;
    }

    protected virtual void OnButtonPressed()
    {
        renderers.ForEach(a => a.UpdateCanPress());
    }

    public abstract void Render(InventoryData inventory);

    protected abstract void InitButton(InventoryCardRenderer renderer);
}
