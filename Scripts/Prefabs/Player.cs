using Godot;
using System.Collections.Generic;
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
                RegisterCommand(_leftMoveCommand);
            }
            if (Input.IsActionJustPressed("ui_right"))
            {
                if (IsMoving()) return;
                _rightMoveCommand.Execute(this);
                RegisterCommand(_rightMoveCommand);
            }
            if (Input.IsActionJustPressed("ui_up"))
            {
                if (IsMoving()) return;
                _upMoveCommand.Execute(this);
                RegisterCommand(_upMoveCommand);
            }
            if (Input.IsActionJustPressed("ui_down"))
            {
                if (IsMoving()) return;
                _downMoveCommand.Execute(this);
                RegisterCommand(_downMoveCommand);
            }

            if (Input.IsActionJustPressed("Z"))
            {
                if (IsMoving()) return;
                Master.UndoCommandEvent();
                UpdateUi();
            }

            if (Input.IsActionJustPressed("X"))
            {
                if (IsMoving()) return;
                Master.RedoCommandEvent();
                UpdateUi();
            }

            if (Input.IsActionJustPressed("R"))
            {
                Master.ResetCurrentLevelEvent();
            }
        }

        public override void MoveTo(Direction dir)
        {
            base.MoveTo(dir);
            Master.PlayerLastDirection = dir;
        }

        public override void UpdateUi()
        {
            foreach (KeyValuePair<Direction, Sprite2D> kvp in _directionSprites)
            {
                string directionPath = DirectionToString(kvp.Key);
                _directionSprites[kvp.Key].Texture = GD.Load($"res://Assets/FunctionSprites/{directionPath}/White.tres") as Texture2D;
            }
            
            foreach (FunctionBlockInfo functionBlockInfo in FunctionBlockInfos.Values)
            {
                string directionPath = DirectionToString(functionBlockInfo.Direction);
                string typePath = FunctionBlockTypeToString(functionBlockInfo.FunctionBlockType);
                _directionSprites[functionBlockInfo.Direction].Texture = GD.Load($"res://Assets/FunctionSprites/{directionPath}/{typePath}.tres") as Texture2D;
            }
        }

        public void EatFunctionBlock(EatFunctionBlockCommand eatFunctionBlockCommand)
        {
            RegisterCommand(eatFunctionBlockCommand);
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

    public class EatFunctionBlockCommand : ICommand
    {
        public Player Player;
        public FunctionBlockInfo FunctionBlockInfo;
        public Actor.Direction Direction;
        public LevelInfo LevelInfo;
        
        // actor 是 FunctionBlock
        public void Execute(Actor actor)
        {
            FunctionBlockInfo.SetDirection(Direction);
            
            Player.FunctionBlockInfos[Direction] = FunctionBlockInfo;
            
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

        // 但这里的 actor 是 player
        public void Undo(Actor actor)
        {
            FunctionBlockInfo.SetDirection(Direction);
            LevelInfo.SetGotFunctionBlockCount(LevelInfo.GetGotFunctionBlockCount() - 1);
            
            actor.FunctionBlockInfos.Remove(Direction);
            actor.UndoPublic();
        
            if (FunctionBlockInfo.FunctionBlock != null)
            {
                FunctionBlockInfo.FunctionBlock.Show();
                FunctionBlockInfo.FunctionBlock.BlockHidden = false;
                FunctionBlockInfo.FunctionBlock.UpdateUi();
                FunctionBlockInfo.FunctionBlock.Enabled();
            }
        }
    }
}

