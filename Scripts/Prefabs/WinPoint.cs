using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts.Prefabs;

public partial class WinPoint : Actor
{
    [Export]
    private Area2D _area2D;
    private bool _isEntered = false;
    
    public override void _Ready()
    {
        base._Ready();
        _area2D.AreaEntered += _area2D_AreaEntered;
    }

    private void _area2D_AreaEntered(Area2D area)
    {
        if (_isEntered) return;
        
        if (area.Owner is Player == false)
        {
            return;
        }
        
        if (Master.CanEnterNextLevel == false)
        {
            return;
        }

        _isEntered = true;
        Master.GetInstance().EnterNextLevelEvent();
    }
}
