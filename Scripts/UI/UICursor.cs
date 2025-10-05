using Godot;
using System;

public partial class UICursor : Control
{
    public static UICursor Current { get; private set; }

    [Export] private TextureRect Renderer { get; set; }

    public Draggable HeldDraggable { get; private set; }

    [Signal]
    public delegate void OnPickedUpDraggableEventHandler(UICursor cursor, Draggable draggable);

    [Signal]
    public delegate void OnDroppedDraggableEventHandler(UICursor cursor, Draggable draggable, ADropPoint dropPoint);

    [Signal]
    public delegate void OnCancelledDraggableEventHandler(UICursor cursor, Draggable draggable);

    public override void _Ready()
    {
        base._Ready();
        Current = this;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            Position = mouseMotionEvent.Position;
        }
    }

    public void PickUpDraggable(Draggable draggable)
    {
        if (HeldDraggable != null)
        {
            GD.PushError("[UICursor] : Already holding a draggable! Curr: " + HeldDraggable + ", new: " + draggable);
            return;
        }
        HeldDraggable = draggable;
        Renderer.Texture = draggable.DragIcon;
        Renderer.Visible = true;
        draggable.PickedUp();
        EmitSignal(SignalName.OnPickedUpDraggable, this, draggable);
        SoundController.Current.PlaySFX("PickUp");
    }

    public void DropDraggable(Draggable draggable, ADropPoint dropPoint)
    {
        if (HeldDraggable != draggable)
        {
            GD.PushError("[UICursor] : Dropping wrong draggable! Curr: " + HeldDraggable + ", new: " + draggable);
            return;
        }
        Renderer.Visible = false;
        dropPoint.Drop(draggable);
        draggable.Dropped();
        EmitSignal(SignalName.OnDroppedDraggable, this, draggable, dropPoint);
        HeldDraggable = null;
        SoundController.Current.PlaySFX("Drop");
    }

    public void CancelPickUp()
    {
        if (HeldDraggable == null)
        {
            GD.PushError("[UICursor] : Canceling no draggable!");
            return;
        }
        Renderer.Visible = false;
        HeldDraggable.Cancelled();
        EmitSignal(SignalName.OnCancelledDraggable, this, HeldDraggable);
        HeldDraggable = null;
    }
}
