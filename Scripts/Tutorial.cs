using Godot;
using System;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class Tutorial : Control
{
    [Export] private Button _closeBtn;

    public override void _Ready()
    {
        _closeBtn.Pressed += delegate
        {
            Master.GetInstance().SawTutorial = true;
            Master.GetInstance().OpenSettingUi = false;
            Owner.QueueFree();
        };
        
        Master.GetInstance().OpenSettingUi = true;
    }
}
