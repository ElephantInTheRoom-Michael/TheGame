using Godot;

namespace TheGame.parts.interfaces.dsky;

public partial class DskyInterface : CanvasLayer
{
    [Export]
    public Container Display { get; set; }
    [Export]
    public Container Keyboard { get; set; }
}