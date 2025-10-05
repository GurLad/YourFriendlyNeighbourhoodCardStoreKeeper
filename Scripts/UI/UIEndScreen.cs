using Godot;
using System;
using System.Collections.Generic;

public partial class UIEndScreen : Node
{
    private List<(int Thresold, string Text)> collectionMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Any% speedrun"),
        (1, "Am I a joke to you?"),
        (5, "Filthy casual"),
        (10, "Barely a collection"),
        (20, "Getting there"),
        (30, "Impressive!"),
        (40, "A winner is you!"),
    };
    private List<(int Thresold, string Text)> foilMessages = new List<(int Thresold, string Text)>()
    {
        (0, "They actually exist trust"),
        (1, "A tiny amount of bling"),
        (5, "Getting curly"),
        (10, "Almost enough for a draft deck"),
        (20, "Rainbow addiction"),
        (30, "Okay you can stop now"),
        (40, "You're positively insane"),
    };
    private List<(int Thresold, string Text)> boughtMessages = new List<(int Thresold, string Text)>()
    {
        (0, "True criminal"),
        (1, "Occasional spender"),
        (5, "Players love you"),
        (10, "Your wallet hates you"),
        (20, "Who cares about rent?"),
        (40, "Gotta spend 'em all"),
    };
    private List<(int Thresold, string Text)> stolenMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Do you even have any cards?"),
        (5, "Dabbling thief"),
        (10, "Now that's robbery"),
        (30, "Join the dark side"),
        (60, "Yes, embrace it"),
        (90, "POWER! UNLIMITED POWER!"),
    };
    private List<(int Thresold, string Text)> soldMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Your soul is pure"),
        (1, "Your soul is tainted"),
        (5, "This is your final warning"),
        (10, "You belong in hell"),
        (20, "You belong in super hell"),
        (40, "You belong in super hell 2"),
        (80, "You belong in super hell 2 deluxe remastered"),
    };
}
