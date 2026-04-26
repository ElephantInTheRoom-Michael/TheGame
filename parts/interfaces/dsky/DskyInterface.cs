using System.Collections.Generic;
using Godot;

namespace TheGame.parts.interfaces.dsky;

public partial class DskyInterface : Node
{
    private const int ERROR_CATEGORY = 0b_001_000_000_000_000_000;
    private const int ERROR_SUBCATEGORY = 0b_001_000_000_000_000;
    
    private const int BUS_ERROR = 0b_001 * ERROR_CATEGORY;
    private const int BUS_ERROR_WRITE_FAILED = BUS_ERROR | 0b_010 * ERROR_SUBCATEGORY;
    private const int BUS_ERROR_WRITE_CONFLICT = BUS_ERROR | 0b_011 * ERROR_SUBCATEGORY;
    
    [Export]
    public DskyDataBus DataBus { get; set; }
    
    [Export]
    public int Target { get; set; }
    
    public int Program { get; set; }
    
    public List<int> Error { get; } = [];
    
    private bool _activeTarget = false;

    public override void _Ready()
    {
        DataBus.TargetChanged += OnDskyTargetChanged;
        DataBus.ProgramBeginWrite += WriteProgram;
        DataBus.ProgramChanged += VerifyProgram;
    }
    
    private void OnDskyTargetChanged(int target)
    {
        _activeTarget = target == Target;
    }

    private void WriteProgram()
    {
        if (!_activeTarget) return;
        DataBus.Program = Program;
    }
    
    private void VerifyProgram(int program)
    {
        if (!_activeTarget) return;
        // Check for a 0 where there should be a 1 indicating the bit could not be pulled high.
        CheckCondition((Program & ~program) == 0, BUS_ERROR_WRITE_FAILED);
        // Check for a 1 where there should be a 0 indicating something else pulled the bit high.
        CheckCondition((~Program & program) == 0, BUS_ERROR_WRITE_CONFLICT);
    }

    private void CheckCondition(bool condition, int relatedError)
    {
        if (condition)
        {
            Error.Remove(relatedError);
        }
        else
        {
            Error.Add(relatedError);
        }

        if (Error.Count == 0)
        {
            DataBus.RemoveTargetError(Target);
        }
        else
        {
            DataBus.AddTargetError(Target);
        }
    }
}