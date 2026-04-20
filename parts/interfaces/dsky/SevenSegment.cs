using System.Linq;
using Godot;

namespace TheGame.parts.interfaces.dsky;

[Tool]
public partial class SevenSegment : Panel
{
    private readonly int[][] _segments = [
        [2, 3, 5, 6, 7, 8, 9, 0],
        [2, 3, 4, 5, 6, 8, 9],
        [2, 3, 5, 6, 8, 9, 0],
        [4, 5, 6, 8, 9, 0],
        [2, 6, 8, 0],
        [1, 2, 3, 4, 7, 8, 9, 0],
        [1, 3, 4, 5, 6, 7, 8, 9, 0],
    ];
    
    private int _digit = 0;
    
    [Export]
    public int Digit
    {
        get => _digit;
        set
        {
            _digit = value;

            GetNode<Line2D>("TopLine").Visible = _segments[0].Contains(_digit);
            GetNode<Line2D>("MiddleLine").Visible = _segments[1].Contains(_digit);
            GetNode<Line2D>("BottomLine").Visible = _segments[2].Contains(_digit);
            GetNode<Line2D>("LeftUpperLine").Visible = _segments[3].Contains(_digit);
            GetNode<Line2D>("LeftLowerLine").Visible = _segments[4].Contains(_digit);
            GetNode<Line2D>("RightUpperLine").Visible = _segments[5].Contains(_digit);
            GetNode<Line2D>("RightLowerLine").Visible = _segments[6].Contains(_digit);
        } 
    }
}