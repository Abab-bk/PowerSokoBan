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
    [Export] private TextureButton _storeBtn;
    [Export] private Control _settingPanel;
    [Export] private Control _storePanel;
    
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
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            Master.GetInstance().OpenSettingUi = true;
            _settingPanel.Show();
        };
        _storeBtn.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            _storePanel.Show();
            Master.GetInstance().OpenSettingUi = true;
        };

        Master.GetInstance().ShowPopupEvent = (popupInfo) =>
        {
            Popup popup = GD.Load<PackedScene>("res://Scenes/Popup.tscn").Instantiate<Popup>();
            popup.PopupInfo = popupInfo;
            AddChild(popup);
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
