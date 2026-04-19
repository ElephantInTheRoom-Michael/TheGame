using Godot;

namespace TheGame;

public partial class RcsComputer : Node2D
{
    [Export]
    private RigidBody2D _ship;
    
    private bool _holdPrograde = false;
    
    private void HoldPrograde()
    {
        _holdPrograde = true;
    }
    
    private void OnVelocityDirectionReceived(double direction)
    {
        if (_holdPrograde)
        {
            GD.Print($"Holding prograde with direction: ${direction}");
            _ship.GlobalRotation = direction;
        }
    }
}