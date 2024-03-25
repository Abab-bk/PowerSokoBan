using System;
using Godot.Collections;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Prefabs.Components;

namespace PowerSokoBan.Scripts.Prefabs;

using Godot;

[GlobalClass]
public partial class Actor : Godot.Node2D
{
    private int _moveDistance = 100;
    private bool _moving = false;
    
    private CommandPool _commandPool;
    private RayCast2D _rayCast2D;

    private Dictionary<Direction, Vector2> _inputs = new Dictionary<Direction, Vector2>()
    {
        {Direction.Up, Vector2.Up},
        {Direction.Down, Vector2.Down},
        {Direction.Left, Vector2.Left},
        {Direction.Right, Vector2.Right},
    };
    
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
    
    public override void _Ready()
    {
        _rayCast2D = new RayCast2D();
        _commandPool = new CommandPool(this);
        
        AddChild(_rayCast2D);
        
        Position = Position.Snapped(Vector2.One * _moveDistance);
    }

    public async void MoveTo(Direction dir)
    {
        if (!AllowMoveTo(dir)) return;
        if (_moving) return;
        Vector2 newPos = GlobalPosition + _inputs[dir] * _moveDistance;
        Tween tween = CreateTween();
        _moving = true;
        tween.TweenProperty(this, "global_position", newPos, 0.2f);
        await ToSignal(tween, "finished");
        _moving = false;
    }

    public bool AllowMoveTo(Direction dir)
    {
        _rayCast2D.TargetPosition = (_inputs[dir] * _moveDistance);
        _rayCast2D.ForceRaycastUpdate();

        if (_rayCast2D.IsColliding())
        {
            var collider = _rayCast2D.GetCollider();
            if (collider is ActorBody)
            {
                ActorBody actorBody = (ActorBody) collider;
                if (actorBody.Actor is Box == false)
                {
                    return false;
                }

                Box boxCollider = (Box)actorBody.Actor;
                boxCollider.MoveTo(dir);
                return true;
            }
            return false;
        }
        return true;
    }

    protected void RegisterCommand(ICommand command)
    {
        _commandPool.Register(command);
    }
    
    protected void Undo()
    {
        _commandPool.Undo();
    }
    
    protected void Redo()
    {
        _commandPool.Redo();
    }
    
}