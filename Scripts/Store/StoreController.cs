using Godot;
using System;
using System.Collections.Generic;

public partial class StoreController : Node
{
    private static StoreController Instance;

    public override void _Ready()
    {
        base._Ready();
        if (Instance != null)
        {
            Instance.QueueFree();
        }
        Instance = this;
    }

    private void SetPaused(bool paused)
    {
        var process_setters = new List<string>()
        {
            "set_process",
            "set_physics_process",
            "set_process_input",
            "set_process_unhandled_input",
            "set_process_unhandled_key_input",
            "set_process_shortcut_input"
        };

        process_setters.ForEach(a => PropagateCall(a, [!paused]));
    }

    public static void Pause() => Instance.SetPaused(true);
    public static void Resume() => Instance.SetPaused(false);
}
