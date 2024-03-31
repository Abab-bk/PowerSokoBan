using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using PowerSokoBan.Scripts.Classes;

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
                if (IsMoving()) return;
                _leftMoveCommand.Execute(this);
                Master.SaveMapEvent(GlobalPosition);
            }
            if (Input.IsActionJustPressed("ui_right"))
            {
                if (IsMoving()) return;
                _rightMoveCommand.Execute(this);
                Master.SaveMapEvent(GlobalPosition);
            }
            if (Input.IsActionJustPressed("ui_up"))
            {
                if (IsMoving()) return;
                _upMoveCommand.Execute(this);
                Master.SaveMapEvent(GlobalPosition);
            }
            if (Input.IsActionJustPressed("ui_down"))
            {
                if (IsMoving()) return;
                _downMoveCommand.Execute(this);
                Master.SaveMapEvent(GlobalPosition);
            }

            if (Input.IsActionJustPressed("Z"))
            {
                if (IsMoving()) return;
                Master.LoadMapEvent(this);
                UpdateUi();
            }

            if (Input.IsActionJustPressed("X"))
            {
                if (IsMoving()) return;
                UpdateUi();
            }

            if (Input.IsActionJustPressed("R"))
            {
                Master.ResetCurrentLevelEvent();
            }
        }

        public override void MoveTo(Direction dir)
        {
            Master.PlayerLastDirection = dir;
            base.MoveTo(dir);
        }

        public override void UpdateUi()
        {
            foreach (KeyValuePair<Direction, Sprite2D> kvp in _directionSprites)
            {
                string directionPath = DirectionToString(kvp.Key);
                _directionSprites[kvp.Key].Texture = null;
            }
            
            foreach (FunctionBlockInfo functionBlockInfo in FunctionBlockInfos.Values)
            {
                string directionPath = DirectionToString(functionBlockInfo.Direction);
                string typePath = functionBlockInfo.FunctionBlockValue switch
                {
                    1 => "White",
                    2 => "Red",
                    3 => "Blue",
                    999 => "Yellow",
                    _ => "White",
                };

                _directionSprites[functionBlockInfo.Direction].Texture = GD.Load($"res://Assets/FunctionSprites/{directionPath}/{typePath}.tres") as Texture2D;
            }
        }
    }


    public class LeftMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Left);
        }
    }

    public class RightMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Right);
        }
    }

    public class UpMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Up);
        }
    }

    public class DownMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            actor.MoveTo(Actor.Direction.Down);
        }
    }
}

