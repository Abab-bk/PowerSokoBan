﻿using System.Threading.Tasks;
using Godot.Collections;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs.Components;

namespace PowerSokoBan.Scripts.Prefabs;

using Godot;

[GlobalClass]
public partial class Actor : Node2D
{
    protected readonly Master Master = Master.GetInstance();

    private int _moveDistance = 64;
    private bool _moving;
    public bool BlockHidden;
    
    private CommandPool _commandPool;

    private Dictionary<Direction, Vector2> _inputs = new Dictionary<Direction, Vector2>()
    {
        {Direction.Up, Vector2.Up},
        {Direction.Down, Vector2.Down},
        {Direction.Left, Vector2.Left},
        {Direction.Right, Vector2.Right},
    };
    
    public System.Collections.Generic.Dictionary<Direction, FunctionBlockInfo> FunctionBlockInfos;

    protected void SetCommandPool(Actor actor)
    {
        _commandPool = new CommandPool(actor);
    }

    public void RulePosition()
    {
        GlobalPosition = GlobalPosition.Snapped(Vector2.One * _moveDistance);
        GlobalPosition += Vector2.One * _moveDistance / 2;
    }
    
    private int GetMoveDistance(Direction dir)
    {
        if (FunctionBlockInfos.ContainsKey(dir)) return (_moveDistance * FunctionBlockInfos[dir].FunctionBlockValue);
        return _moveDistance;
    }
    
    protected bool IsMoving()
    {
        return _moving;
    }
    
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
    
    public override void _Ready()
    {
        AddToGroup("Actors");
        FunctionBlockInfos = new System.Collections.Generic.Dictionary<Direction, FunctionBlockInfo>();
        _commandPool = new CommandPool(this);
        
        RulePosition();
    }

    public virtual void UpdateUi()
    {
        Master.UpdateUiEvent();
    }

    public virtual async void MoveTo(Direction dir)
    {
        if (!AllowMoveTo(dir))
        {
            Vector2 originalPos = GlobalPosition;
            Tween shockTween = CreateTween();
            _moving = true;
            
            shockTween.TweenProperty(this, "global_position", new Vector2(GlobalPosition.X - 20, GlobalPosition.Y), 0.05f);
            shockTween.TweenProperty(this, "global_position", new Vector2(GlobalPosition.X + 20, GlobalPosition.Y), 0.05f);
            shockTween.TweenProperty(this, "global_position", new Vector2(GlobalPosition.X + 20, GlobalPosition.Y), 0.05f);
            shockTween.TweenProperty(this, "global_position", originalPos, 0.05f);

            await ToSignal(shockTween, "finished");
            _moving = false;
            return;
        }

        if (_moving) return;
        
        Vector2 newPos = GlobalPosition + _inputs[dir] * GetMoveDistance(dir);
        
        Tween tween = CreateTween();
        
        _moving = true;
        tween.TweenProperty(this, "global_position", newPos, 0.2f);
        await ToSignal(tween, "finished");
        _moving = false;
    }

    private bool AllowMoveTo(Direction dir)
    {
        Actor actor = Master.GetActorByPosEvent(GlobalPosition + _inputs[dir] * GetMoveDistance(dir));

        if (actor is null)
        {
            return true;
        }
        
        if (actor is FunctionBlock)
        {
            if (FunctionBlockInfos.ContainsKey(dir))
                return false;
            if (actor.BlockHidden) return true;

            return true;
        }

        if (actor is WinPoint) return true;
        if (actor is Wall) return false;
        
        return false;
    }

    protected string DirectionToString(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return "UpFunctions";
            case Direction.Down:
                return "DownFunctions";
            case Direction.Left:
                return "LeftFunctions";
            case Direction.Right:
                return "RightFunctions";
        }
        
        return "UpFunctions";
    }

    protected string FunctionBlockTypeToString(FunctionBlockType type)
    {
        switch (type)
        {
            case FunctionBlockType.Red:
                return "Red";
            case FunctionBlockType.Blue:
                return "Blue";
            case FunctionBlockType.Green:
                return "Green";
            case FunctionBlockType.Yellow:
                return "Yellow";
        }
        
        return "Red";
    }
}
