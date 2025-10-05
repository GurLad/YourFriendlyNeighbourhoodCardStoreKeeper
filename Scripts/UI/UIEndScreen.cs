using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UIEndScreen : Control
{
    private static List<(int Thresold, string Text)> collectionMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Any% speedrun"),
        (1, "Am I a joke to you?"),
        (5, "Filthy casual"),
        (10, "Barely a collection"),
        (20, "Getting there"),
        (30, "Impressive!"),
        (40, "A winner is you!"),
    };
    private static List<(int Thresold, string Text)> foilMessages = new List<(int Thresold, string Text)>()
    {
        (0, "They actually exist trust"),
        (1, "A tiny amount of bling"),
        (5, "Getting curly"),
        (10, "Almost enough for a draft deck"),
        (20, "Rainbow addiction"),
        (30, "Okay you can stop now"),
        (40, "You're positively insane"),
    };
    private static List<(int Thresold, string Text)> boughtMessages = new List<(int Thresold, string Text)>()
    {
        (0, "True criminal"),
        (1, "Occasional spender"),
        (5, "Players love you"),
        (10, "Your wallet hates you"),
        (20, "Who cares about rent?"),
        (40, "Gotta spend 'em all"),
    };
    private static List<(int Thresold, string Text)> stolenMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Do you even have any cards?"),
        (5, "Dabbling thief"),
        (10, "Now that's robbery"),
        (30, "Join the dark side"),
        (60, "Yes, embrace it"),
        (90, "POWER! UNLIMITED POWER!"),
    };
    private static List<(int Thresold, string Text)> soldMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Your soul is pure"),
        (1, "Your soul is tainted"),
        (5, "This is your final warning"),
        (10, "You belong in hell"),
        (20, "You belong in super hell"),
        (40, "You belong in super hell 2"),
        (80, "You belong in super hell 2 deluxe remastered"),
    };
    private static List<(int Thresold, string Text)> detected = new List<(int Thresold, string Text)>()
    {
        (0, "Swift as the wind"),
        (1, "\"It was an accident I swear!\""),
        (3, "Better find an ace attorney"),
        (5, "Enjoy your collection in jail!"),
    };

    private List<(Func<int>, string, List<(int Thresold, string Text)>)> parts = new List<(Func<int>, string, List<(int Thresold, string Text)>)>()
    {
        (() => PlayerInventoryController.CountUnique(), " Cards Collected", collectionMessages),
        (() => PlayerInventoryController.CountUniqueFoil(), " Foils Collected", foilMessages),
        (() => Stats.CardsStolen, " Cards Stolen", stolenMessages),
        (() => Stats.TimesDetected, " Times Detected", detected),
    };

    [Export] private PackedScene scenePart;
    [Export] private Control partHolder;
    [Export] private float openCloseTime = 0.6f;
    [Export] private Button showButton;
    [Export] private Button hideButton;

    private Interpolator interpolator;
    private bool visible;
    private List<UIEndScreenPart> screenParts = new List<UIEndScreenPart>();
    private int current = 0;

    public override void _Ready()
    {
        base._Ready();
        interpolator = new Interpolator();
        AddChild(interpolator);

        PivotOffset = new Vector2(504.0f, 352.0f) / 2;
        showButton.Pressed += ShowM;
        hideButton.Pressed += HideM;

        parts.ForEach(a =>
        {
            UIEndScreenPart part = scenePart.Instantiate<UIEndScreenPart>();
            part.Clear();
            screenParts.Add(part);
        });
    }

    public void ShowM()
    {
        if (interpolator.Active)
        {
            GD.PushWarning("Wah");
            return;
        }
        screenParts.ForEach(a => a.Clear());
        current = 0;
        visible = true;
        interpolator.Interpolate(openCloseTime,
            new Interpolator.InterpolateObject(
                a => Scale = Vector2.One * a,
                Scale.X,
                1,
                Easing.EaseOutBack
            ));
        interpolator.OnFinish = ShowPart;
    }

    public void HideM()
    {
        if (interpolator.Active)
        {
            GD.PushWarning("Wah");
            return;
        }
        interpolator.Interpolate(openCloseTime,
            new Interpolator.InterpolateObject(
                a => Scale = Vector2.One * a,
                Scale.X,
                0,
                Easing.EaseOutBack
            ));
        interpolator.OnFinish = () => Visible = false;
    }

    private void ShowPart()
    {
        if (current >= screenParts.Count)
        {
            return;
        }
        int val = parts[current].Item1();
        int index = 0;
        while (val >= parts[current].Item3[index].Thresold && index + 1 < parts[current].Item3.Count) index++;
        screenParts[current].Show(val + parts[current].Item2, parts[current].Item3[index].Text, 0, ShowPart);
        current++;
    }
}
