using Godot;
using System;
using System.Collections.Generic;

public partial class CardsLoader : Node
{
    private static CardsLoader Instance;

    [Export] private Texture2D missingArt;

    private List<CardData> cards = new List<CardData>()
    {
        // new CardData(
        //     "",
        //     Rarity.Rare,
        //     "",
        //     "",
        //     "",
        //     0,
        //     0,
        //     0
        // ),
        new CardData(
            "Hidden Squirrel",
            Rarity.Common,
            "HiddenSquirrel",
            "Instant",
            "Create a tapped and attacking 1/1 squirrel token.",
            2
        ),
        new CardData(
            "Fluffy Pirate",
            Rarity.Common,
            "FluffyPirate",
            "Hamster",
            "\a\"Yaaaaaar! I'm a-pirate!\"",
            2,
            2,
            2
        ),
        new CardData(
            "Frog Rider",
            Rarity.Common,
            "FrogRider",
            "Hamster",
            "\a\"Is it a frog? Is it a rider? No, it's a frog rider!\"",
            5,
            6,
            4
        ),
        new CardData(
            "Haunted Hallway",
            Rarity.Common,
            "HauntedHallway",
            "Instant",
            "Attacking creatures get -2/-2 this turn.",
            3
        ),
        new CardData(
            "Underpaid Intern",
            Rarity.Common,
            "PostMorterm",
            "Hamster",
            "Tap to draw a card and flip a coin. If you lose the flip, this hamster dies.",
            3,
            1,
            2
        ),
        new CardData(
            "A New Tail",
            Rarity.Common,
            "Abomination",
            "Sorcery",
            "Put a +1/+1 counter on any target.",
            1
        ),
        new CardData(
            "Swordmaster",
            Rarity.Common,
            "LegendMercenary",
            "Hamster",
            "\a\"How much weight... Can you handle...?\"",
            2,
            3,
            1
        ),
        new CardData(
            "Round & Round",
            Rarity.Common,
            "RoundAndRound",
            "Enchantment",
            "Tap enchanted creature at the beginning of each turn.",
            2
        ),
        new CardData(
            "Puzzling Puzzle",
            Rarity.Common,
            "PuzzlingPuzzle",
            "Instant",
            "Counter target spell unless its controller pays 3 mana.",
            3
        ),
        new CardData(
            "Go Ham",
            Rarity.Common,
            "GoHam",
            "Instant",
            "Attacking creatures get +3/+0 this turn.",
            4
        ),
        new CardData(
            "Dramster",
            Rarity.Common,
            "Dramster",
            "Hamster",
            "When Dramster deals damage, gain that much life.",
            6,
            7,
            4
        ),
        new CardData(
            "Winter Nap",
            Rarity.Common,
            "WinterNap",
            "Instant",
            "Return target creature to its owner's hand.",
            1
        ),
        new CardData(
            "Mysterious Ninja",
            Rarity.Common,
            "MysteriousNinja",
            "Instant Squirrel",
            "",
            4,
            5,
            1
        ),
        new CardData(
            "Pet Attack",
            Rarity.Common,
            "PetAttack",
            "Sorcery",
            "Each player creates two 0/0 Human tokens.",
            1
        ),
        new CardData(
            "Flying Fiend",
            Rarity.Common,
            "FlyingFiend",
            "Squirrel",
            "Flying",
            2,
            1,
            2
        ),
        new CardData(
            "Colossal Dreadrel",
            Rarity.Common,
            "ColossalDreadmaw",
            "Squirrel",
            "Trample (This creature can deal excess combat damage to the player it's attacking.)",
            6,
            6,
            6
        ),
        new CardData(
            "Hamster Artificer",
            Rarity.Uncommon,
            "HamsterArtificer",
            "Hamster",
            "Tap to create a 1/1 Robot token.",
            3,
            1,
            1
        ),
        new CardData(
            "Unlikely Allies",
            Rarity.Uncommon,
            "UnlikelyAllies",
            "Sorcery",
            "Return a Hamster and a Squirrel from your graveyard to the battlefield.",
            5
        ),
        new CardData(
            "Gemstone Mine",
            Rarity.Uncommon,
            "GemstoneMine",
            "Item",
            "Tap to add three mana.",
            6
        ),
        new CardData(
            "Night Ambush",
            Rarity.Uncommon,
            "NightAmbush",
            "Instant",
            "Destroy target attacking or blocking creature.",
            1
        ),
        new CardData(
            "Dread Summoner",
            Rarity.Uncommon,
            "DreadSummoner",
            "Hamster",
            "When summoned, create a 1/1 Skeleton token.",
            1,
            0,
            3
        ),
        new CardData(
            "Proper Burial",
            Rarity.Uncommon,
            "ProperBurial",
            "Sorcery",
            "Exile all graveyards.",
            2
        ),
        new CardData(
            "HAMSTR/X",
            Rarity.Uncommon,
            "HAMSTRIX",
            "Sorcery",
            "Summon three 3/1 Hamster tokens.",
            4
        ),
        new CardData(
            "Werrel",
            Rarity.Uncommon,
            "Werrel",
            "Hamsquirrel",
            "Werrel is both a Hamster and a Squirrel.",
            3,
            3,
            3
        ),
        new CardData(
            "Backstab",
            Rarity.Uncommon,
            "Backstab",
            "Instant",
            "Target attacking creature fights another target attacking creature.",
            3
        ),
        new CardData(
            "Squirrelled Away",
            Rarity.Uncommon,
            "SquirrelledAway",
            "Sorcery",
            "Gain control of target Item.",
            3
        ),
        new CardData(
            "Sudden Adoption",
            Rarity.Uncommon,
            "SuddenAdoption",
            "Instant",
            "Gain control of target summon spell.",
            4
        ),
        new CardData(
            "Not So Fast!",
            Rarity.Uncommon,
            "NotSoFast",
            "Instant",
            "Counter target spell.",
            3
        ),
        new CardData(
            "Mister Muster",
            Rarity.Uncommon,
            "MisterMuster",
            "Hamster Legend",
            "Tap to gain 3 life.",
            5,
            3,
            5
        ),
        new CardData(
            "Yi'Xyzquz",
            Rarity.Uncommon,
            "Yi'Xyzquz",
            "Ky'Rzoquz",
            "\a\"The invasion begins.\n\nNext winter.\"",
            7,
            9,
            6
        ),
        new CardData(
            "Masked Bard",
            Rarity.Uncommon,
            "MaskedBard",
            "Squirrel",
            "Tap to untap any target.",
            2,
            0,
            4
        ),
        new CardData(
            "Very Tall Guy",
            Rarity.Uncommon,
            "VeryTallGuy",
            "Squirrel",
            "When Very Tall Guy dies, create 15 1/1 Squirrel tokens.",
            6,
            0,
            1
        ),
        new CardData(
            "Sword of Light",
            Rarity.Rare,
            "SwordOfLight",
            "Equipment Item",
            "Equipped creature gains +2/+2 for each Hamster you control.",
            5
        ),
        new CardData(
            "Darkness Blade",
            Rarity.Rare,
            "Blade of Darkness",
            "Equipment Item",
            "Choose a number X. Equipped creature and creatures you don't control both get +X/+X.",
            5
        ),
        new CardData(
            "Horror",
            Rarity.Rare,
            "Horror",
            "Sorcery",
            "A player takes 1 damage, discards 2 cards, sacrifices 3 creatures, and loses the game",
            9
        ),
        new CardData(
            "The One Acron",
            Rarity.Rare,
            "TheOneAcron",
            "Item",
            "Tap The One Acron to draw any number of cards and take that much damage.",
            4
        ),
        new CardData(
            "El, Realmtraveler",
            Rarity.Rare,
            "EllaRealmtraveler",
            "Realmtraveler",
            "Tap to start a subgame. The loser of the subgame loses 20 life.",
            7,
            7,
            7
        ),
        new CardData(
            "Happy Rainbows",
            Rarity.Rare,
            "HappyRainbows",
            "Hamster",
            "When summoned, destroy all other creatures.",
            6,
            9,
            1
        ),
        new CardData(
            "Dark McShadow",
            Rarity.Rare,
            "DarknessMcShadow",
            "Squirrel",
            "When summoned, gain 20 life.",
            6,
            1,
            9
        ),
        new CardData(
            "Dante",
            Rarity.Rare,
            "Dante",
            "Dante",
            "\a\"Devil May Cry 6 is now available at a GameStop near you!\"",
            3,
            2,
            2
        ),
    };

    public override void _Ready()
    {
        base._Ready();
        if (Instance != null)
        {
            QueueFree();
            return;
        }
        Instance = this;

        cards.ForEach(a => a.Art = GetArt(a.ArtPath));
        cards.ForEach((a, i) => a.ID = i);
    }

    public static int Count => Instance.cards.Count;

    public static CardData GetCard(int id) => Instance.cards[id];

    public static List<CardData> GetAllCards() => Instance.cards.ConvertAll(a => a);

    public static Texture2D GetArt(string artPath)
    {
        string path = "res://Sprites/CardArt/" + artPath + ".png";
        if (ResourceLoader.Exists(path)) return ResourceLoader.Load<Texture2D>(path);
        return Instance.missingArt;
    }
}
