using Godot;
using System;
using System.Diagnostics;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;
using PowerSokoBan.Scripts.Prefabs.Components;

namespace PowerSokoBan.Scripts.Prefabs;


public partial class FunctionBlock : Actor
{
    [Export] private Area2D ActorArea { get; set; }

    private int _functionBlockValue = 2;
    private FunctionBlockType _functionBlockType = FunctionBlockType.Red;
    private Direction _functionBlockDirection = Direction.Up;
    
    private FunctionBlockInfo _functionBlockInfo;
    
    public override void _Ready()
    {
        if (ActorArea == null)
        {
            Debug.Assert(false, "ActorArea 为空");
        }
        
        ActorArea.AreaEntered += _actorArea_AreaEntered;
        _functionBlockInfo = new FunctionBlockInfo(_functionBlockValue, _functionBlockType, _functionBlockDirection);
    }


    private void _actorArea_AreaEntered(Area2D area)
    {
        if (area.Owner is Player == false)
        {
            GD.Print("进入的 Area 不是 Player 的 Area");
            return;
        }
        
        Player player = (Player) area.Owner;
        
        _functionBlockInfo.SetDirection(Master.PlayerLastDirection);
        player.AddFunctionBlock(_functionBlockInfo, Master.PlayerLastDirection);
        QueueFree();
    }
}
