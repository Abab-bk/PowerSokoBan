using Godot;
using System;
using System.Diagnostics;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Enums;

namespace PowerSokoBan.Scripts.Prefabs;


public partial class FunctionBlock : Actor
{
    [Export] private Area2D ActorArea { get; set; }
    [Export] private Sprite2D _sprite2D;
    [Export] private CollisionShape2D _collisionShape2D;

    public LevelInfo LevelInfo;
    
    public int Id;
    
    private int _functionBlockValue = 2;
    private FunctionBlockType _functionBlockType = FunctionBlockType.Red;
    private Direction _functionBlockDirection = Direction.Up;
    
    public FunctionBlockInfo FunctionBlockInfo;

    private EatFunctionBlockCommand _eatenCommand;

    public void LoadFunctionBlockMapInfo(FunctionBlockMapInfo mapInfo, Player player)
    {
        GlobalPosition = mapInfo.Pos;

        if (mapInfo.BlockHidden == BlockHidden)
        {
            return;
        }
        
        if (BlockHidden)
        {
            player.FunctionBlockInfos.Remove(FunctionBlockInfo.Direction);
            Show();
            Enabled();
            BlockHidden = mapInfo.BlockHidden;
            LevelInfo.ReduceGotFunctionBlockCount(1);
        }
        else
        {
            player.FunctionBlockInfos.Add(FunctionBlockInfo.Direction, FunctionBlockInfo);
            Hide();
            Disabled();
            BlockHidden = mapInfo.BlockHidden;
            LevelInfo.AddGotFunctionBlockCount(1);
        }
        
        Master.UpdateUiEvent();
    }

    private void Disabled()
    {
        _collisionShape2D.SetDeferred("disabled", true);
    }

    private void Enabled()
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
        FunctionBlockInfo = new FunctionBlockInfo(_functionBlockValue, _functionBlockType, _functionBlockDirection);
        
        switch (_functionBlockValue)
        {
            case 0:
                _sprite2D.Texture = GD.Load<AtlasTexture>("res://Assets/Tokens/Box.tres");
                break;
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

        // switch (_functionBlockType)
        // {
        //     case FunctionBlockType.Yellow:
        //         _sprite2D.Texture = GD.Load<AtlasTexture>("res://Assets/Tokens/Mouth.tres");
        //         break;
        //     case FunctionBlockType.Green:
        //         _sprite2D.Texture = GD.Load<AtlasTexture>("res://Assets/Tokens/Box.tres");
        //         break;
        // }
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
        
        FunctionBlockInfo.FunctionBlock = this;

        _eatenCommand.Player = player;
        _eatenCommand.FunctionBlockInfo = FunctionBlockInfo;
        _eatenCommand.Direction = Master.PlayerLastDirection;
        _eatenCommand.LevelInfo = LevelInfo;
        _eatenCommand.Execute(this);
    }
    
    public class EatFunctionBlockCommand : ICommand
    {
        public Player Player;
        public FunctionBlockInfo FunctionBlockInfo;
        public Direction Direction;
        public LevelInfo LevelInfo;
        
        public void Execute(Actor actor)
        {
            FunctionBlockInfo.SetDirection(Direction);

            if (FunctionBlockInfo.FunctionBlockValue == 0)
            {
                // 是箱子
                Player.FunctionBlockInfos.Remove(Direction);
            }
            else
            {
                Player.FunctionBlockInfos[Direction] = FunctionBlockInfo;
            }
            
            LevelInfo.AddGotFunctionBlockCount(1);

            if (FunctionBlockInfo.FunctionBlock != null)
            {
                actor.Hide();
                actor.BlockHidden = true;
                actor.UpdateUi();
                ((FunctionBlock) actor).Disabled();
            }
            
            Player.UpdateUi();
        }
    }
}
