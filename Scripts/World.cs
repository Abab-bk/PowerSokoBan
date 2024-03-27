using Godot;
using System;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Classes.Factorys;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts;

public partial class World : Node2D
{
    [Export]
    private Node2D _functionBlocks;

    public override void _Ready()
    {
        foreach (var mark in _functionBlocks.GetChildren())
        {
            if (mark is Marker2D == false)
            {
                continue;
            }
            
            var mark2D = (Marker2D) mark;
            AddFunctionBlocks(mark2D.GlobalPosition, 2, ActorType.FunctionBlockBlue, Actor.Direction.Down);
        }
    }

    private void AddFunctionBlocks(Vector2 pos, int value, ActorType actorType, Actor.Direction direction)
    {
        var functionBlock = ActorFactory.CreateActor(actorType, value, direction);
        if (functionBlock == null)
        {
            return;
        }
        _functionBlocks.AddChild(functionBlock);
        functionBlock.GlobalPosition = pos;
        functionBlock.RulePosition();
    }
}
