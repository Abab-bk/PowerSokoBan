using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class Ui : Control
{
    [Export]
    private Label _label;
    [Export] private TextureButton _undoButton;
    [Export] private TextureButton _restartButton;
    [Export] private TextureButton _settingBtn;
    [Export] private Control _settingPanel;
    
    private LevelInfo _levelInfo;
    
    public override void _Ready()
    {
        _undoButton.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            Master.GetInstance().LoadMapEvent(Master.GetInstance().Player);
        };
        _restartButton.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            Master.GetInstance().ResetCurrentLevelEvent();
        };
        _settingBtn.Pressed += delegate
        {
            Master.GetInstance().OpenSettingUi = true;
            _settingPanel.Show();
        };
    }

    public void SetLevelInfo(LevelInfo levelInfo)
    {
        _levelInfo = levelInfo;
    }

    public void UpdateUi()
    {
        // 第 1 关   收集：1/2
        _label.Text = $"\ud83d\udcf4\ud83d\udcb3 {_levelInfo.Id}  🔑 {_levelInfo.GetGotFunctionBlockCount()}/{_levelInfo.GetTotalFunctionBlockCount()}";
    }
}
