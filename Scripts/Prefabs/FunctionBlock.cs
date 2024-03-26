using Godot;
using System;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Prefabs;

public partial class FunctionBlock : Actor
{
    [Export]
    public Area2D _area2D;
    
    private int _functionBlockValue;
    private FunctionBlockType _functionBlockType;
    private Direction _functionBlockDirection;
    
    private FunctionBlockInfo _functionBlockInfo;
    
    public override void _Ready()
    {
        _area2D.BodyEntered += _area2D_BodyEntered;
        _functionBlockInfo = new FunctionBlockInfo(_functionBlockValue, _functionBlockType, _functionBlockDirection);
    }

    private void _area2D_BodyEntered(Node body)
    {
        if (body is Player == false)
        {
            return;
        }
        
        Player player = (Player) body;
        player.AddFunctionBlock(_functionBlockInfo, _functionBlockDirection);
    }
}
