using Godot;
using System;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Classes.Factorys;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts;

public partial class World : Node2D
{
    [Export] private LevelGenerator _levelGenerator;
    [Export] private Ui _ui;
    
    public override void _Ready()
    {
        LevelInfo levelInfo = _levelGenerator.GenerateLevel("intro1b.txt");
        if (levelInfo != null)
        {
            _ui.SetLevelInfo(levelInfo);
            UpdateUi();
        }
        
        Master.GetInstance().UpdateUiEvent += UpdateUi;
    }

    private void UpdateUi()
    {
        _ui.UpdateUi();
    }
}

