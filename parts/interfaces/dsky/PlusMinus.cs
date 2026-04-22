using Godot;

namespace TheGame.parts.interfaces.dsky;

public partial class PlusMinus : Panel
{
    [Export]
    public int Polarity
    {
        get => _polarity;
        set
        {
            _polarity = value;
            GetNode<Line2D>("PlusLine").Visible = _polarity > 0;
            GetNode<Line2D>("MinusLine").Visible = _polarity != 0;
        }
    }

    private int _polarity;
}