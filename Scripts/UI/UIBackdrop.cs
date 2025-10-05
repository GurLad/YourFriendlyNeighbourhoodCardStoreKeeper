using Godot;
using System;

public partial class UIBackdrop : Node
{
    public override void _Notification(int what)
    {
        base._Notification(what);
        if (what == 22) //NOTIFICATION_DRAG_END)
        {
            if (!GetViewport().GuiIsDragSuccessful())
            {
                UICursor cursor = UICursor.Current;
                cursor.CancelPickUp();
            }
        }
    }
}
