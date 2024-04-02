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
    }

    public void SetLevelInfo(LevelInfo levelInfo)
    {
        _levelInfo = levelInfo;
    }

    public void UpdateUi()
    {
        // 第 1 关   收集：1/2
        _label.Text = $"第 {_levelInfo.Id} 关   收集：{_levelInfo.GetGotFunctionBlockCount()}/{_levelInfo.GetTotalFunctionBlockCount()}";
    }
}
