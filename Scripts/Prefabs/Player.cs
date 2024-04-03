using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Prefabs.Components;

namespace PowerSokoBan.Scripts.Prefabs
{
    public partial class Player : Actor
    {
        [Export] private SwipeDetector _swipeDetector;
        
        private Godot.Collections.Dictionary<Direction, Sprite2D> _directionSprites;
        
        private LeftMoveCommand _leftMoveCommand;
        private RightMoveCommand _rightMoveCommand;
        private UpMoveCommand _upMoveCommand;
        private DownMoveCommand _downMoveCommand;
        
        public override void _Ready()
        {
            base._Ready();
            Master.Player = this;
            
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
            
            _swipeDetector.Swiped += MoveCommandByDir;
            _swipeDetector.SwipedCanceled += delegate { };
        }

        private void MoveCommandByDir(Vector2 dir)
        {
            if (Master.OpenSettingUi) return;
            
            if (dir == Vector2.Left)
            {
                _rightMoveCommand.Execute(this);
                return;
            }

            if (dir == Vector2.Right)
            {
                _leftMoveCommand.Execute(this);
                return;
            }

            if (dir == Vector2.Up)
            {
                _downMoveCommand.Execute(this);
                return;
            }

            if (dir == Vector2.Down)
            {
                _upMoveCommand.Execute(this);
            }
        }
        
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (Input.IsActionJustPressed("ui_left"))
            {
                _leftMoveCommand.Execute(this);
            }
            if (Input.IsActionJustPressed("ui_right"))
            {
                _rightMoveCommand.Execute(this);
            }
            if (Input.IsActionJustPressed("ui_up"))
            {
                _upMoveCommand.Execute(this);
            }
            if (Input.IsActionJustPressed("ui_down"))
            {
                if (IsMoving()) return;
                _downMoveCommand.Execute(this);
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
            if (actor.IsMoving()) return;
            actor.MoveTo(Actor.Direction.Left);
            Master.GetInstance().SaveMapEvent(actor.GlobalPosition, actor.FunctionBlockInfos);
        }
    }

    public class RightMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            if (actor.IsMoving()) return;
            actor.MoveTo(Actor.Direction.Right);
            Master.GetInstance().SaveMapEvent(actor.GlobalPosition, actor.FunctionBlockInfos);
        }
    }

    public class UpMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            if (actor.IsMoving()) return;
            actor.MoveTo(Actor.Direction.Up);
            Master.GetInstance().SaveMapEvent(actor.GlobalPosition, actor.FunctionBlockInfos);
        }
    }

    public class DownMoveCommand : ICommand
    {
        public void Execute(Actor actor)
        {
            if (actor.IsMoving()) return;
            actor.MoveTo(Actor.Direction.Down);
            Master.GetInstance().SaveMapEvent(actor.GlobalPosition, actor.FunctionBlockInfos);
        }
    }
}

