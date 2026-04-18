using Godot;

namespace TheGame;

public partial class LocationComputer : Node2D
{
    [Signal]
    public delegate void LocationEventHandler();
    
    private void OnTimerTimeout()
    {
        EmitSignal(SignalName.Location, GlobalPosition);
    }
}