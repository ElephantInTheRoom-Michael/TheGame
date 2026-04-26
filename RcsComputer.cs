using Godot;
using TheGame.parts.interfaces.dsky;

namespace TheGame;

public partial class RcsComputer : Node2D
{
    [Export] 
    public RigidBody2D Ship;
    
    [Export] 
    public DskyInterface DskyInterface;

    private bool _holdPrograde = false;

    private int _dskyTarget = 10;

    public override void _Ready()
    {
        base._Ready();
        
        DskyInterface.Program = 0;
    }

    private void HoldPrograde()
    {
        _holdPrograde = true;
        DskyInterface.Program = 1;
    }

    private void OnVelocityDirectionReceived(double direction)
    {
        if (_holdPrograde)
        {
            GD.Print($"Holding prograde with direction: ${direction}");
            Ship.GlobalRotation = direction;
        }
    }
}
