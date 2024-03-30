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
    [Export] private CollisionShape2D _collisionShape2D;

    public LevelInfo LevelInfo;
    
    private int _functionBlockValue = 2;
    private FunctionBlockType _functionBlockType = FunctionBlockType.Red;
    private Direction _functionBlockDirection = Direction.Up;
    
    private FunctionBlockInfo _functionBlockInfo;

    private EatFunctionBlockCommand _eatenCommand;

    public void Disabled()
    {
        _collisionShape2D.SetDeferred("disabled", true);
    }

    public void Enabled()
    {
        _collisionShape2D.SetDeferred("disabled", false);
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
        _eatenCommand = new EatFunctionBlockCommand();
        
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
        
        if (BlockHidden) return;
        
        Player player = (Player) area.Owner;
        
        _functionBlockInfo.FunctionBlock = this;

        _eatenCommand.Player = player;
        _eatenCommand.FunctionBlockInfo = _functionBlockInfo;
        _eatenCommand.Direction = Master.PlayerLastDirection;
        _eatenCommand.LevelInfo = LevelInfo;
        _eatenCommand.Execute(this);
        
        // 仅仅是注册
        player.EatFunctionBlock(_eatenCommand);
    }
}
