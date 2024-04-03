using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class Setting : Control
{
    [Export] private Button _closeBtn;
    [Export] private Button _backToMainMenuBtn;
    [Export] private Button _changeMusicBtn;

    private bool _enabledMusic = true;
    
    private const string SavePath = "user://setting.cfg";
    
    public override void _Ready()
    {
        _closeBtn.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            Master.GetInstance().OpenSettingUi = false;
            Hide();
        };
        _backToMainMenuBtn.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            Master.GetInstance().OpenSettingUi = false;
            Master.GetInstance().BackToMainMenuEvent();
        };
        _changeMusicBtn.Pressed += delegate
        {
            Master master = Master.GetInstance();
            master.PlaySoundEvent(Audios.Click);
            
            SettingMusic();
            
            SaveGame();
        };

        if (LoadGame())
        {
            SettingMusic();
        }
    }

    private void SaveGame()
    {
        ConfigFile config = new ConfigFile();
        config.SetValue("System", "EnabledMusic", _enabledMusic);
        config.Save(SavePath);
    }

    private void SettingMusic()
    {
        _enabledMusic = !_enabledMusic;

        if (_enabledMusic)
        {
            _changeMusicBtn.Text = "\ud83d\ude45";
            AudioServer.SetBusVolumeDb(0, -80f);
        }
        else
        {
            _changeMusicBtn.Text = "\ud83d\ude46";
            AudioServer.SetBusVolumeDb(0, 0f);
        }
    }

    private bool LoadGame()
    {
        ConfigFile config = new ConfigFile();
        Error error = config.Load(SavePath);

        if (error != Error.Ok) return false;
        
        _enabledMusic = (bool)config.GetValue("System", "EnabledMusic", true);
        
        return true;
    }
}
