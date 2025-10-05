using Godot;
using System;

public partial class Draggable : Control
{
    private enum HighlightMode
    {
        None = 0,
        Held = 1,
        Hover = 2
    }

    [ExportCategory("Vars")]
    [Export] public Texture2D DragIcon { get; protected set; }
    [ExportCategory("Vars - Highlight")]
    [Export] private Color BaseModulate { get; set; } = Colors.White;
    [Export] private float HeldOpacity { get; set; } = 0.6f;
    [Export] private Color HoverOutline { get; set; } = Colors.White;

    public bool CanDrag { get; set; } = true;

    private HighlightMode Highlight;
    private ShaderMaterial ShaderMaterial;

    [Signal]
    public delegate void OnPickedUpEventHandler();

    [Signal]
    public delegate void OnDroppedEventHandler();

    [Signal]
    public delegate void OnCancelledEventHandler();

    public override void _Ready()
    {
        base._Ready();

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    protected Material InitMaterial(Material material)
    {
        material = (Material)material.Duplicate();
        ShaderMaterial = material is ShaderMaterial sm ? sm : null;
        if (ShaderMaterial == null)
        {
            GD.PushError("[ADraggable]: No shader material!");
            GetTree().Quit();
        }
        ShaderMaterial.Set("outlineColor", HoverOutline);
        RenderHighlight();
        return material;
    }

    public void RenderHighlight(bool force = false)
    {
        if (!CanDrag && !force)
        {
            return;
        }
        else if (!CanDrag)
        {
            Highlight = HighlightMode.None;
        }
        ShaderMaterial.SetShaderParameter("modulate", new Color(BaseModulate, (Highlight & HighlightMode.Held) != HighlightMode.None ? HeldOpacity : 1));
        ShaderMaterial.SetShaderParameter("showOutline", (Highlight & HighlightMode.Hover) != HighlightMode.None && UICursor.Current.HeldDraggable == null);
    }

    protected virtual void OnMouseEntered()
    {
        if (UICursor.Current.HeldDraggable != null)
        {
            return;
        }
        Highlight |= HighlightMode.Hover;
        RenderHighlight();
    }

    protected virtual void OnMouseExited()
    {
        Highlight &= ~HighlightMode.Hover;
        RenderHighlight();
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        if (!CanDrag)
        {
            return this;
        }
        OnMouseExited();
        Highlight |= HighlightMode.Held;
        RenderHighlight();
        UICursor.Current.PickUpDraggable(this);
        return UICursor.Current;
    }

    public virtual void PickedUp()
    {
        EmitSignal(SignalName.OnPickedUp);
    }

    public virtual void Dropped()
    {
        EmitSignal(SignalName.OnDropped);
    }

    public virtual void Cancelled()
    {
        Highlight &= ~HighlightMode.Held;
        RenderHighlight();
        EmitSignal(SignalName.OnCancelled);
    }
}
