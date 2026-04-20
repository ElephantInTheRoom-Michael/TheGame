using System;
using Godot;

namespace TheGame.parts.interfaces.dsky;

public partial class DisplayKeyboard : Node2D
{
    [Export]
    public DskyInterface Interface { get; set; }

    public int Target { get; private set; }
    public int Program { get; set; }
    public int Verb { get; private set; }
    public int Noun { get; private set; }

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
    
    private Vector3I _data = Vector3I.Zero;

    private int DataOne
    {
        get => _data.X;
        set
        {
            _data.X = value;
            ShowData(0);
        }
    }
    private int DataTwo
    {
        get => _data.X;
        set
        {
            _data.X = value;
            ShowData(1);
        }
    }
    private int DataThree
    {
        get => _data.X;
        set
        {
            _data.X = value;
            ShowData(2);
        }
    }

    private static readonly Action<int> EmptyUpdateNumber = _ => { };
    private Action<int> _updateNumber = EmptyUpdateNumber;

    public override void _Ready()
    {
        for (var i = 0; i < 10; i++)
        {
            var buttonIndex = i;
            Interface.Keyboard.GetNode<Button>($"Button{buttonIndex}").Pressed += () => OnNumberKey(buttonIndex);
        }
        
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
    }

    private void OnVerbEntryStart()
    {
        GD.Print("Verb entry started");
        Verb = 0;
        _updateNumber = n => Verb = UpdateShortNumber(Verb, n);
    }
    
    private void OnNounEntryStart()
    {
        GD.Print("Noun entry started");
        Noun = 0;
        _updateNumber = n => Noun = UpdateShortNumber(Noun, n);
    }
    
    private void OnDataEntryStart(int i)
    {
        GD.Print($"Data {i + 1} entry started");
        _updateNumber = i switch
        {
            0 => n => DataOne = UpdateLongNumber(DataOne, n),
            1 => n => DataTwo = UpdateLongNumber(DataTwo, n),
            2 => n => DataThree = UpdateLongNumber(DataThree, n),
            _ => EmptyUpdateNumber
        };

        _updateNumber(0);
    }

    private void OnNumberKey(int n)
    {
        GD.Print($"Number key pressed: {n}");
        _updateNumber(n);
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
    }
    
    private void UpdateTarget(int n)
    {
        Target = (Target * 10) + n;
    }

    private int UpdateShortNumber(int number, int n)
    {
        return ((number * 10) + n) % 1000;
    }
    
    private int UpdateLongNumber(int number, int n)
    {
        return ((number * 10) + n) % 1000000;
    }
    
    private void ShowData(int i)
    {
        var remaining = _data[i];
        for (var d = 6; d > 0; d--)
        {
            var digit = remaining % 10;
            remaining /= 10;
            Interface.Display.GetNode<SevenSegment>($"Data{i + 1}SevenSegment{d}").Digit = digit;
        }
    }
}