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
    [Export] private Sprite2D _sprite2D;

    public LevelInfo LevelInfo;
    
    private int _functionBlockValue = 2;
    private FunctionBlockType _functionBlockType = FunctionBlockType.Red;
    private Direction _functionBlockDirection = Direction.Up;
    
    private FunctionBlockInfo _functionBlockInfo;

    private EatenCommand _eatenCommand;

    public void Disabled()
    {
        ActorArea.GetChild(0).SetDeferred("disabled", true);
    }

    public void Enabled()
    {
        ActorArea.GetChild(0).SetDeferred("disabled", false);
    }
    
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
        
        switch (_functionBlockValue)
        {
            case 1:
                _sprite2D.Texture = GD.Load<AtlasTexture>("res://Assets/Tokens/White1.tres");
                break;
            case 2:
                _sprite2D.Texture = GD.Load<AtlasTexture>("res://Assets/Tokens/Red2.tres");
                break;
            case 3:
                _sprite2D.Texture = GD.Load<AtlasTexture>("res://Assets/Tokens/Blue3.tres");
                break;
        }
    }

    private void _actorArea_AreaEntered(Area2D area)
    {
        if (area.Owner is Player == false)
        {
            GD.Print("进入的 Area 不是 Player 的 Area");
            return;
        }
        
        Player player = (Player) area.Owner;
        
        // if (player.IsMoving())
        // {
        //     return;
        // }

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
        actor.BlockHidden = true;
        actor.UpdateUi();

        if (actor is FunctionBlock)
        {
            ((FunctionBlock) actor).Disabled();
        }
    }

    public void Undo(Actor actor)
    {
        FunctionBlockInfo.SetDirection(Direction);
        LevelInfo.SetGotFunctionBlockCount(LevelInfo.GetGotFunctionBlockCount() - 1);
        actor.Show();
        actor.BlockHidden = false;
        actor.UpdateUi();
        
        if (actor is FunctionBlock)
        {
            ((FunctionBlock) actor).Enabled();
        }
    }
}