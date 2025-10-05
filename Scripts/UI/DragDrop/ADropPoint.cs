using Godot;
using System;

public abstract partial class ADropPoint : Control
{
    private enum HighlightMode
    {
        None = 0,
        CanDrop = 1,
        Hover = 2,
        Empty = 4,
        CanDropHover = CanDrop | Hover
    }

    [Export] private Sprite2D Sprite;
    [ExportCategory("Highlight")]
    [Export] private Color EmptyOutline { get; set; } = Colors.Transparent;
    [Export] private Color CanDropOutline { get; set; } = Colors.White;
    [Export] private Color HoverOutline { get; set; } = Colors.Yellow;

    private HighlightMode Highlight;
    private ShaderMaterial ShaderMaterial;

    public Color ShaderModulate
    {
        set => ShaderMaterial.SetShaderParameter("modulate", value);
    }

    public override void _Ready()
    {
        base._Ready();
        UICursor.Current.OnPickedUpDraggable += CursorPickedUpDraggable;
        UICursor.Current.OnDroppedDraggable += CursorDroppedDraggable;
        UICursor.Current.OnCancelledDraggable += CursorCancelledDraggable;
        Sprite.Material = (Material)Sprite.Material.Duplicate();
        ShaderMaterial = Sprite.Material is ShaderMaterial sm ? sm : null;
        if (ShaderMaterial == null)
        {
            GD.PushError("[UIDraggable]: No shader material!");
            GetTree().Quit();
        }
        Highlight |= HighlightMode.Empty;

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    public void RenderHighlight()
    {
        ShaderMaterial.SetShaderParameter("showOutline", Highlight != HighlightMode.None && Highlight != HighlightMode.Empty);
        ShaderMaterial.SetShaderParameter("outlineColor",
            (Highlight & HighlightMode.Hover) != HighlightMode.None ?
                HoverOutline :
                ((Highlight & HighlightMode.CanDrop) != HighlightMode.None ?
                    CanDropOutline :
                    EmptyOutline));
    }

    private void CursorPickedUpDraggable(UICursor cursor, Draggable draggable)
    {
        if (CanDrop(draggable))
        {
            Highlight |= HighlightMode.CanDrop;
            RenderHighlight();
        }
    }

    private void CursorDroppedDraggable(UICursor cursor, Draggable draggable, ADropPoint dropPoint)
    {
        Highlight &= ~HighlightMode.CanDropHover;
        RenderHighlight();
    }

    private void CursorCancelledDraggable(UICursor cursor, Draggable draggable)
    {
        Highlight &= ~HighlightMode.CanDropHover;
        RenderHighlight();
    }

    protected void OnMouseEntered()
    {
        if (UICursor.Current.HeldDraggable != null && CanDrop(UICursor.Current.HeldDraggable))
        {
            Highlight |= HighlightMode.Hover;
            RenderHighlight();
        }
    }

    protected void OnMouseExited()
    {
        Highlight &= ~HighlightMode.Hover;
        RenderHighlight();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        UICursor.Current.OnPickedUpDraggable -= CursorPickedUpDraggable;
        UICursor.Current.OnDroppedDraggable -= CursorDroppedDraggable;
        UICursor.Current.OnCancelledDraggable -= CursorCancelledDraggable;
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.As<GodotObject>() is UICursor cursor && cursor.HeldDraggable != null && CanDrop(cursor.HeldDraggable))
        {
            return true;
        }
        return false;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        if (data.As<GodotObject>() is UICursor cursor)
        {
            if (cursor.HeldDraggable != null && CanDrop(cursor.HeldDraggable))
            {
                Highlight &= ~HighlightMode.Empty;
                cursor.DropDraggable(cursor.HeldDraggable, this);
            }
            else
            {
                GD.PushError("[AUIGrave]: Placing invalid Draggable!");
            }
        }
        else
        {
            GD.PushError("[AUIGrave]: Placing non-Draggables!");
        }
    }

    public abstract void Drop(Draggable draggable);

    protected abstract bool CanDrop(Draggable draggable);
}
