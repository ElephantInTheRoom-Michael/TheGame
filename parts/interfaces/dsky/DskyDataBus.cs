using Godot;

namespace TheGame.parts.interfaces.dsky;

public partial class DskyDataBus : Node
{
    [Signal]
    public delegate void TargetBeginWriteEventHandler();
    [Signal]
    public delegate void TargetChangedEventHandler(int target);

    [Signal]
    public delegate void ProgramBeginWriteEventHandler();
    [Signal]
    public delegate void ProgramChangedEventHandler(int program);
    
    public int Clock { get; private set; }

    public int Target
    {
        get => _target;
        set
        {
            GD.Print($"Set target on bus to {value}");
            _target |= value;
            GD.Print($"Target on bus is {_target}");
        }
    }

    public int Program
    {
        get => _program;
        set
        {
            GD.Print($"Set program on bus to {value}");
            _program |= value;
            GD.Print($"Program on bus is {_program}");
        }
    }

    private int _target;
    private int _program;

    private void OnClockTick()
    {
        EmitSignal(SignalName.TargetChanged, Target);
        _target = 0;
        EmitSignal(SignalName.TargetBeginWrite);
        
        Clock = (Clock + 1) % 4;

        switch (Clock)
        {
            case 0:
                ClockZeroBegin();
                break;
            case 1:
                ClockZeroEnd();
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    private void ClockZeroBegin()
    {
        _program = 0;
        EmitSignal(SignalName.ProgramBeginWrite);
    }
    
    private void ClockZeroEnd()
    {
        EmitSignal(SignalName.ProgramChanged, Program);
    }
}