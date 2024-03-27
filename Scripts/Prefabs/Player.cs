using Godot;
using System;
using System.Collections.Generic;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Prefabs
{
    public partial class Player : Actor
    {
        private Godot.Collections.Dictionary<Direction, Sprite2D> _directionSprites;
        
        private LeftMoveCommand _leftMoveCommand;
        private RightMoveCommand _rightMoveCommand;
        private UpMoveCommand _upMoveCommand;
        private DownMoveCommand _downMoveCommand;

        public override void _Ready()
        {
            base._Ready();

            Master.EnterNextLevelEvent += EnterNextLevel;
            
            _directionSprites = new Godot.Collections.Dictionary<Direction, Sprite2D>()
            {
                {Direction.Up, GetNode<Sprite2D>("Node2D/Node2D/UpFunction")},
                {Direction.Down, GetNode<Sprite2D>("Node2D/Node2D/DownFunction")},
                {Direction.Left, GetNode<Sprite2D>("Node2D/Node2D/LeftFunction")},
                {Direction.Right, GetNode<Sprite2D>("Node2D/Node2D/RightFunction")},
            };
            
            _leftMoveCommand = new LeftMoveCommand();
            _rightMoveCommand = new RightMoveCommand();
            _upMoveCommand = new UpMoveCommand();
            _downMoveCommand = new DownMoveCommand();
        }
        
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (Input.IsActionJustPressed("ui_left"))
            {
                _leftMoveCommand.Execute(this);
                RegisterCommand(_leftMoveCommand);
            }
            if (Input.IsActionJustPressed("ui_right"))
            {
                _rightMoveCommand.Execute(this);
                RegisterCommand(_rightMoveCommand);
            }
            if (Input.IsActionJustPressed("ui_up"))
            {
                _upMoveCommand.Execute(this);
                RegisterCommand(_upMoveCommand);
            }
            if (Input.IsActionJustPressed("ui_down"))
            {
                _downMoveCommand.Execute(this);
                RegisterCommand(_downMoveCommand);
            }

            if (Input.IsActionJustPressed("Z"))
            {
                Undo();
            }

            if (Input.IsActionJustPressed("X"))
            {
                Redo();
            }
        }

        public override void MoveTo(Direction dir)
        {
            base.MoveTo(dir);
            Master.PlayerLastDirection = dir;
        }

        protected override void UpdateUi()
        {
            foreach (FunctionBlockInfo functionBlockInfo in FunctionBlockInfos.Values)
            {
                String directionPath = DirectionToString(functionBlockInfo.Direction);
                String typePath = FunctionBlockTypeToString(functionBlockInfo.FunctionBlockType);
                _directionSprites[functionBlockInfo.Direction].Texture = GD.Load($"res://Assets/FunctionSprites/{directionPath}/{typePath}.tres") as Texture2D;
            }
        }

        public void EnterNextLevel()
        {
            if (Master.CanEnterNextLevel == false)
            {
                return;
            }
            GD.Print("进入下一层");
            GlobalPosition = Vector2.Zero;
        }
    }


    public class LeftMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Left);
        }

        public void Undo(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Right);
        }
    }

    public class RightMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Right);
        }

        public void Undo(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Left);
        }
    }

    public class UpMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Up);
        }

        public void Undo(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Down);
        }
    }

    public class DownMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Down);
        }

        public void Undo(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Up);
        }
    }
}

