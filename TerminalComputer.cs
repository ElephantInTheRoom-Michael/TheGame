using Godot;

namespace TheGame;

public partial class TerminalComputer : Node2D
{
    [Export]
    private RichTextLabel _output;

    private void OnDataInput(Variant data)
    {
        _output.AppendText(data.ToString());
        _output.Newline();
    }
}