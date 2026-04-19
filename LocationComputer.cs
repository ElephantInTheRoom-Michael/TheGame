using Godot;

namespace TheGame;

public partial class LocationComputer : Node2D
{
    [Export]
    private RigidBody2D _ship;
    
    [Signal]
    public delegate void LocationEventHandler(Vector2 location);

    [Signal]
    public delegate void VelocityDirectionEventHandler(double direction);
    
    private void OnTimerTimeout()
    {
        EmitSignal(SignalName.Location, GlobalPosition);
        EmitSignal(SignalName.VelocityDirection, _ship.LinearVelocity.Angle());
    }
}