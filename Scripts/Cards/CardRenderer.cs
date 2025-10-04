using Godot;
using System;

public partial class CardRenderer : Node
{
    [ExportCategory("Nodes")]
    [Export] private Label Name { get; set; }
    [Export] private Label Type { get; set; }
    [Export] private Label Desc { get; set; }
    [Export] private Label Mana { get; set; }
    [Export] private Label Power { get; set; }
    [Export] private Label Toughness { get; set; }

    [Export] private TextureRect Image;
    [Export] private TextureRect CardFrame;
    [Export] private TextureRect FoilFilter;

    [ExportCategory("Resources")]
    [Export] private Texture2D[] RarityFrames;
    [Export] private Texture2D[] JunkRarityFrames;
    [Export] private Texture2D[] MaskRarityFrames;

    [ExportCategory("Vars")]
    [Export] private Color FlavourTextModulate;
    [Export] private Color NormalTextModulate;

    public void Render(CardData data)
    {
        Name.Text = data.Name;
        Type.Text = data.Type;
        string desc = data.Desc;
        if (desc.StartsWith("\a"))
        {
            Desc.SelfModulate = FlavourTextModulate;
            desc = desc.Substring(1);
        }
        else
        {
            Desc.SelfModulate = NormalTextModulate;
        }
        Desc.Text = desc;
        Mana.Text = data.Cost.ToString();
        Power.Text = data.Power.ToString();
        Toughness.Text = data.Toughness.ToString();
        Image.Texture = data.Art;
        FoilFilter.Visible = data.Foil;
        FoilFilter.Texture = MaskRarityFrames[(int)data.Rarity];
        CardFrame.Texture = (data.Junk ? JunkRarityFrames : RarityFrames)[(int)data.Rarity];
    }
}
