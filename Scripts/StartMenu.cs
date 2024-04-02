using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class StartMenu : Control
{
    [Export] private TextureButton _startButton;
    [Export] private TextureButton _exitButton;
    [Export] private AudioStreamPlayer _audioStreamPlayer;

    public override void _Ready()
    {
        _startButton.Pressed += delegate
        {
            _audioStreamPlayer.Stop();
            SceneManager.ChangeSceneTo(this, "res://Scenes/World.tscn");
        };

        _exitButton.Pressed += delegate
        {
            GetTree().Quit();
        };
        
        _audioStreamPlayer.Play();
    }
}
