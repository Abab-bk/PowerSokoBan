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
        private LeftMoveCommand _leftMoveCommand;
        private RightMoveCommand _rightMoveCommand;
        private UpMoveCommand _upMoveCommand;
        private DownMoveCommand _downMoveCommand;

        public override void _Ready()
        {
            base._Ready();
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

