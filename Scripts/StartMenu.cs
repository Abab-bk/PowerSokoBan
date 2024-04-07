using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class StartMenu : Control
{
    [Export] private TextureButton _startButton;
    [Export] private TextureButton _exitButton;
    [Export] private AudioStreamPlayer _audioStreamPlayer;
    [Export] private Control _selectLevel;
    
    
    public override void _Ready()
    {
        _startButton.Pressed += delegate
        {
            _selectLevel.Show();
        };

        _exitButton.Pressed += delegate
        {
            GetTree().Quit();
        };

        Master.GetInstance().StopAllMusicEvent = delegate
        {
            _audioStreamPlayer.Stop();
        };

        Master.GetInstance().StartMenu = this;
        _audioStreamPlayer.Play();
        _selectLevel.Hide();
    }
}
