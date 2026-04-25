using Godot;
using TheGame.parts.interfaces.dsky;

namespace TheGame;

public partial class RcsComputer : Node2D
{
    [Export]
    private RigidBody2D _ship;
    [Export]
    private DskyDataBus _dskyDataBus;
    
    private bool _holdPrograde = false;
    
    private int _dskyTarget = 10;

    public override void _Ready()
    {
        base._Ready();
        
        _dskyDataBus.TargetChanged += OnDskyTargetChanged;
    }

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
    
    private void OnDskyTargetChanged(int target)
    {
        if (target == _dskyTarget)
        {
            GD.Print("Dsky target is me");
        }
    }
}