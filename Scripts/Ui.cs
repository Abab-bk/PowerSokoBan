using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class Ui : Control
{
    [Export]
    private Label _label;

    private LevelInfo _levelInfo;

    public void SetLevelInfo(LevelInfo levelInfo)
    {
        _levelInfo = levelInfo;
    }

    public void UpdateUi()
    {
        _label.Text = "Level: " + _levelInfo.Id + "  " + $"{_levelInfo.GetGotFunctionBlockCount()} / {_levelInfo.GetTotalFunctionBlockCount()}";
    }
}
