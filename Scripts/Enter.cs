using Godot;
using System;
using PowerSokoBan.Scripts;

public partial class Enter : Control
{
    [Export] bool _showStartCut = true;
    public override void _Ready()
    {
        if (_showStartCut)
        {
            SceneManager.ChangeSceneTo(this, "res://Scenes/StartCut.tscn");
            return;
        }

        if (OS.HasFeature("template"))
        {
            SceneManager.ChangeSceneTo(this, "res://Scenes/StartCut.tscn");
        }
        else
        {
            SceneManager.ChangeSceneTo(this, "res://Scenes/World.tscn");
        }
    }
}
