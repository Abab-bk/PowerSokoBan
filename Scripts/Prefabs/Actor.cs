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
    
    private CommandPool _commandPool;
    private RayCast2D _rayCast2D;

    private Dictionary<Direction, Vector2> _inputs = new Dictionary<Direction, Vector2>()
    {
        {Direction.Up, Vector2.Up},
        {Direction.Down, Vector2.Down},
        {Direction.Left, Vector2.Left},
        {Direction.Right, Vector2.Right},
    };

    private AddFunctionCommand _addFunctionCommand;
    
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

    public void AddFunctionBlock(FunctionBlockInfo functionBlockInfo, Direction direction)
    {
        _addFunctionCommand.FunctionBlockInfo = functionBlockInfo;
        _addFunctionCommand.Direction = direction;
        _addFunctionCommand.Execute(this);
        RegisterCommand(_addFunctionCommand);
        UpdateUi();
    }
    
    private int GetMoveDistance(Direction dir)
    {
        if (FunctionBlockInfos.ContainsKey(dir)) return (_moveDistance * FunctionBlockInfos[dir].FunctionBlockValue);
        return _moveDistance;
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
        Master.UndoCommandEvent += UndoEvent;
        Master.RedoCommandEvent += Redo;
        
        _addFunctionCommand = new AddFunctionCommand();
        FunctionBlockInfos = new System.Collections.Generic.Dictionary<Direction, FunctionBlockInfo>();
        _rayCast2D = new RayCast2D();
        _commandPool = new CommandPool(this);
        
        AddChild(_rayCast2D);

        _rayCast2D.CollisionMask = 0;
        _rayCast2D.SetCollisionMaskValue(2, true);
        
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
            
            shockTween.TweenProperty(this, "global_position", new Vector2(GlobalPosition.X - 20, GlobalPosition.Y), 0.05f);
            shockTween.TweenProperty(this, "global_position", new Vector2(GlobalPosition.X + 20, GlobalPosition.Y), 0.05f);
            shockTween.TweenProperty(this, "global_position", new Vector2(GlobalPosition.X + 20, GlobalPosition.Y), 0.05f);
            shockTween.TweenProperty(this, "global_position", originalPos, 0.05f);
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
        _rayCast2D.TargetPosition = (_inputs[dir] * GetMoveDistance(dir));
        _rayCast2D.ForceRaycastUpdate();

        if (_rayCast2D.IsColliding())
        {
            var collider = _rayCast2D.GetCollider();
            if (collider is ActorBody)
            {
                ActorBody actorBody = (ActorBody) collider;
                if (actorBody.Actor is Box)
                {
                    Box boxCollider = (Box)actorBody.Actor;
                    boxCollider.MoveTo(dir);
                    return true;
                }

                if (actorBody.Actor is FunctionBlock)
                {
                    if (FunctionBlockInfos.ContainsKey(dir))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        return true;
    }

    protected void RegisterCommand(ICommand command)
    {
        _commandPool.Register(command);
    }
    
    private void Undo()
    {
        _commandPool.Undo();
    }
    
    private void Redo()
    {
        _commandPool.Redo();
    }

    protected virtual void UndoEvent()
    {
        Undo();
    }
    
    protected virtual void RedoEvent()
    {
        Redo();
    }

    public void RedoPublic()
    {
        Redo();
    }
    
    public void UndoPublic()
    {
        Undo();
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

public class AddFunctionCommand : ICommand
{
    public FunctionBlockInfo FunctionBlockInfo;
    public Actor.Direction Direction;
    
    public void Execute(Actor actor)
    {
        actor.FunctionBlockInfos[Direction] = FunctionBlockInfo;
        actor.UpdateUi();
    }

    public void Undo(Actor actor)
    {
        actor.FunctionBlockInfos.Remove(Direction);
        actor.UpdateUi();
        actor.UndoPublic();
    }
}