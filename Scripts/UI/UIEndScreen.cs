using Godot;
using System;
using System.Collections.Generic;

public partial class UIEndScreen : Node
{
    private List<(int Thresold, string Text)> collectionMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Any% speedrun"),
        (5, "Am I a joke to you?"),
        (10, "Filthy casual"),
        (20, "Barely a collection"),
        (30, "Getting there"),
        (39, "Impressive!"),
        (40, "A winner is you!"),
    };
    private List<(int Thresold, string Text)> foilMessages = new List<(int Thresold, string Text)>()
    {
        (0, "They actually exist trust"),
        (5, "A tiny amount of bling"),
        (10, "Getting curly"),
        (20, "Almost enough for a draft deck"),
        (30, "Rainbow addiction"),
        (39, "Okay you can stop now"),
        (40, "You're positively insane"),
    };
    private List<(int Thresold, string Text)> boughtMessages = new List<(int Thresold, string Text)>()
    {
        (0, "True criminal"),
        (5, "Occasional spender"),
        (10, "Players love you"),
        (20, "Your wallet hates you"),
        (39, "Who cares about rent?"),
        (40, "Gotta spend 'em all"),
    };
    private List<(int Thresold, string Text)> stolenMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Do you even have any cards?"),
        (10, "Dabbling thief"),
        (20, "Now that's robbery"),
        (40, "Join the dark side"),
        (80, "Yes, embrace it"),
        (81, "POWER! UNLIMITED POWER!"),
    };
    private List<(int Thresold, string Text)> soldMessages = new List<(int Thresold, string Text)>()
    {
        (0, "Your soul is pure"),
        (5, "This is your final warning"),
        (10, "You belong in hell"),
        (20, "You belong in super hell"),
        (40, "You belong in super hell 2"),
        (80, "You belong in super hell 2 deluxe remastered"),
        (81, "You are the devil incarnate"),
    };
}
