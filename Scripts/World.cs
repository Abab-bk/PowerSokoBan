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
    
    public override void _Ready()
    {
        _levelGenerator.GenerateLevel("intro1b.txt");
    }
}
