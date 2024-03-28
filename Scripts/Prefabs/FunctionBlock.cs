using Godot;
using System;
using System.Diagnostics;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;
using PowerSokoBan.Scripts.Prefabs.Components;

namespace PowerSokoBan.Scripts.Prefabs;


public partial class FunctionBlock : Actor
{
    [Export] private Area2D ActorArea { get; set; }

    public LevelInfo LevelInfo;
    
    private int _functionBlockValue = 2;
    private FunctionBlockType _functionBlockType = FunctionBlockType.Red;
    private Direction _functionBlockDirection = Direction.Up;
    
    private FunctionBlockInfo _functionBlockInfo;

    private EatenCommand _eatenCommand;
    
    public void SetFunctionBlockValue(int value)
    {
        _functionBlockValue = value;
    }
    
    public void SetFunctionBlockType(FunctionBlockType type)
    {
        _functionBlockType = type;
    }
    
    public void SetFunctionBlockDirection(Direction direction)
    {
        _functionBlockDirection = direction;
    }

    public override void _Ready()
    {
        base._Ready();
        if (ActorArea == null)
        {
            Debug.Assert(false, "ActorArea 为空");
        }

        SetCommandPool(this);
        _eatenCommand = new EatenCommand();
        
        ActorArea.AreaEntered += _actorArea_AreaEntered;
        _functionBlockInfo = new FunctionBlockInfo(_functionBlockValue, _functionBlockType, _functionBlockDirection);
    }

    private void _actorArea_AreaEntered(Area2D area)
    {
        if (area.Owner is Player == false)
        {
            GD.Print("进入的 Area 不是 Player 的 Area");
            return;
        }
        
        Player player = (Player) area.Owner;
        
        // Eaten Command
        _eatenCommand.Player = player;
        _eatenCommand.FunctionBlockInfo = _functionBlockInfo;
        _eatenCommand.Direction = Master.PlayerLastDirection;
        _eatenCommand.LevelInfo = LevelInfo;
        _eatenCommand.Execute(this);
        RegisterCommand(_eatenCommand);
    }
}


public class EatenCommand : ICommand
{
    public Player Player;
    public FunctionBlockInfo FunctionBlockInfo;
    public Actor.Direction Direction;
    public LevelInfo LevelInfo;
    public void Execute(Actor actor)
    {
        FunctionBlockInfo.SetDirection(Direction);
        Player.AddFunctionBlock(FunctionBlockInfo, Direction);
        LevelInfo.AddGotFunctionBlockCount(1);
        actor.Hide();
        actor.UpdateUi();
    }

    public void Undo(Actor actor)
    {
        FunctionBlockInfo.SetDirection(Direction);
        LevelInfo.SetGotFunctionBlockCount(LevelInfo.GetGotFunctionBlockCount() - 1);
        actor.Show();
        actor.UpdateUi();
    }
}