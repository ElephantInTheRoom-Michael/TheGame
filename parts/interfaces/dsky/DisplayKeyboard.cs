using System;
using Godot;

namespace TheGame.parts.interfaces.dsky;

public partial class DisplayKeyboard : Node2D
{
    [Export]
    public DskyInterface Interface { get; set; }
    [Export]
    public DskyDataBus DataBus { get; set; }

    public int Target
    {
        get => _target;
        private set
        {
            _target = value;
            GD.Print($"Target is set to {_target}");
            ShowNumber(_target, 3, "Target");
            DataBus.Target = _target;
        }
    }

    public int Program
    {
        get => _program;
        set
        {
            _program = value;
            GD.Print($"Program is set to {_program}");
            ShowNumber(_program, 3, "Program");
        } 
    }

    public int Verb
    {
        get => _verb;
        private set
        {
            _verb = value;
            GD.Print($"Verb is set to {_verb}");
            ShowNumber(_verb, 3, "Verb");
        }
    }

    public int Noun
    {
        get => _noun;
        private set
        {
            _noun = value;
            GD.Print($"Noun is set to {_noun}");
            ShowNumber(_noun, 3, "Noun");
        }
    }

    public Vector3I Data
    {
        get => _data;
        set
        {
            DataOne = value.X;
            DataTwo = value.Y;
            DataThree = value.Z;
        }
    }
    
    private int _target;
    private int _program;
    private int _verb;
    private int _noun;
    private Vector3I _data = Vector3I.Zero;

    private int DataOne
    {
        get => _data.X;
        set
        {
            _data.X = value;
            GD.Print($"Data1 is set to {_data.X}");
            ShowNumber(_data.X, 6, "Data1");
            ShowSign(_data.X, "Data1");
        }
    }
    private int DataTwo
    {
        get => _data.Y;
        set
        {
            _data.Y = value;
            GD.Print($"Data2 is set to {_data.Y}");
            ShowNumber(_data.Y, 6, "Data2");
            ShowSign(_data.Y, "Data2");
        }
    }
    private int DataThree
    {
        get => _data.Z;
        set
        {
            _data.Z = value;
            GD.Print($"Data3 is set to {_data.Z}");
            ShowNumber(_data.Z, 6, "Data3");
            ShowSign(_data.Z, "Data3");
        }
    }

    private static readonly Action<int> EmptyUpdateNumber = _ => { };
    private Action<int> _updateNumber = EmptyUpdateNumber;
    private static readonly Action<int> NoSignChange = _ => { };
    private Action<int> _updateSign = NoSignChange;
    
    private bool _negativeZero = false;

    public override void _Ready()
    {
        for (var i = 0; i < 10; i++)
        {
            var buttonIndex = i;
            Interface.Keyboard.GetNode<Button>($"Button{buttonIndex}").Pressed += () => OnNumberKey(buttonIndex);
        }
        Interface.Keyboard.GetNode<Button>("PlusButton").Pressed += () => OnSignChange(1);
        Interface.Keyboard.GetNode<Button>("MinusButton").Pressed += () => OnSignChange(-1);
        
        for (var i = 0; i < 3; i++)
        {
            var dataIndex = i;
            Interface.Keyboard.GetNode<Button>($"DataButton{dataIndex + 1}").Pressed += () => OnDataEntryStart(dataIndex);
        }
        
        Interface.Keyboard.GetNode<Button>("TargetButton").Pressed += OnTargetEntryStart;
        Interface.Keyboard.GetNode<Button>("VerbButton").Pressed += OnVerbEntryStart;
        Interface.Keyboard.GetNode<Button>("NounButton").Pressed += OnNounEntryStart;
        
        Interface.Keyboard.GetNode<Button>("ResetButton").Pressed += Reset;
        
        Reset();
    }
    
    private void OnTargetEntryStart()
    {
        GD.Print("Target entry started");
        Target = 0;
        Program = 0;
        Verb = 0;
        Noun = 0;
        _updateNumber = n => Target = UpdateShortNumber(Target, n);
        _updateSign = NoSignChange;
    }

    private void OnVerbEntryStart()
    {
        GD.Print("Verb entry started");
        Verb = 0;
        _updateNumber = n => Verb = UpdateShortNumber(Verb, n);
        _updateSign = NoSignChange;
    }
    
    private void OnNounEntryStart()
    {
        GD.Print("Noun entry started");
        Noun = 0;
        _updateNumber = n => Noun = UpdateShortNumber(Noun, n);
        _updateSign = NoSignChange;
    }
    
    private void OnDataEntryStart(int i)
    {
        GD.Print($"Data {i + 1} entry started");
        
        // If the user pressed minus on the previous number but did not update it further, the negative zero is not
        // saved. Reset the negative zero flag and refresh the number display to remove the minus sign.
        if (_negativeZero)
        {
            _updateSign(1);
        }
        
        _updateNumber = i switch
        {
            0 => n => DataOne = UpdateLongNumber(DataOne, n),
            1 => n => DataTwo = UpdateLongNumber(DataTwo, n),
            2 => n => DataThree = UpdateLongNumber(DataThree, n),
            _ => EmptyUpdateNumber
        };
        _updateSign = i switch
        {
            0 => polarity => DataOne = UpdateSign(DataOne, polarity),
            1 => polarity => DataTwo = UpdateSign(DataTwo, polarity),
            2 => polarity => DataThree = UpdateSign(DataThree, polarity),
            _ => NoSignChange
        };
    }

    private void OnNumberKey(int n)
    {
        GD.Print($"Number key pressed: {n}");
        _updateNumber(n);
    }
    
    private void OnSignChange(int polarity)
    {
        GD.Print("Sign change");
        _updateSign(polarity);
    }

    private void Reset()
    {
        GD.Print("Reset DSKY");
        Target = 0;
        Program = 0;
        Verb = 0;
        Noun = 0;
        Data = Vector3I.Zero;
        _updateNumber = EmptyUpdateNumber;
        _updateSign = NoSignChange;
        _negativeZero = false;
    }

    private int UpdateNumber(int number, int n, int digits)
    {
        var polarity = number < 0 ? -1 : 1;
        if (number == 0 && _negativeZero && n != 0)
        {
            polarity = -1;
            _negativeZero = false;
        }
        var result = (((Math.Abs(number) * 10) + n) % (int) Math.Pow(10, digits)) * polarity;
        if (result == 0 && polarity < 0)
        {
            _negativeZero = true;
        }
        return result;
    }

    private int UpdateShortNumber(int number, int n)
    {
        return UpdateNumber(number, n, 3);
    }
    
    private int UpdateLongNumber(int number, int n)
    {
        return UpdateNumber(number, n, 6);
    }

    private int UpdateSign(int number, int polarity)
    {
        if (number == 0)
        {
            _negativeZero = polarity < 0;
            return 0;
        }
        return Math.Abs(number) * polarity;
    }
    
    private void ShowNumber(int n, int digits, String sevenSegmentName)
    {
        var remaining = Math.Abs(n);
        for (var d = digits; d > 0; d--)
        {
            var digit = remaining % 10;
            remaining /= 10;
            Interface.Display.GetNode<SevenSegment>($"{sevenSegmentName}SevenSegment{d}").Digit = digit;
        }
    }
    
    private void ShowSign(int n, String plusMinusName)
    {
        var polarity = n == 0 && _negativeZero ? -1 : n;
        Interface.Display.GetNode<PlusMinus>($"{plusMinusName}PlusMinus").Polarity = polarity;
    }
}