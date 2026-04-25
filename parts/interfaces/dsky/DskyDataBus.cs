using Godot;

namespace TheGame.parts.interfaces.dsky;

public partial class DskyDataBus : Node
{
    [Signal]
    public delegate void TargetChangedEventHandler(int target);

    public int Target
    {
        get => _target;
        set
        {
            _target = value;
            EmitSignal(SignalName.TargetChanged, _target);
        }
    }

    private int _target;
}