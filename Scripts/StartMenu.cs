using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class StartMenu : Control
{
    [Export] private TextureButton _startButton;
    [Export] private TextureButton _exitButton;

    public override void _Ready()
    {
        _startButton.Pressed += delegate
        {
            SceneManager.ChangeSceneTo(this, "res://Scenes/World.tscn");
        };

        _exitButton.Pressed += delegate
        {
            GetTree().Quit();
        };
        
        
    }
}
