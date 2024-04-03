using Godot;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class Store : Control
{
    [Export] private GridContainer _storeItems;
    [Export] private Button _closeBtn;

    public override void _Ready()
    {
        _closeBtn.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            Master.GetInstance().OpenSettingUi = false;
            Hide();
        };

        UpdateUi();
    }

    private void UpdateUi()
    {
        foreach (Node storeItem in _storeItems.GetChildren())
        {
            storeItem.QueueFree();
        }
        
        FileAccess file = FileAccess.Open("res://Assets/DataBase/Skins.txt", FileAccess.ModeFlags.Read);
        if (file == null)
        {
            return;
        }
        
        string text = file.GetAsText();
        file.Close();
        
        string[] lines = text.Split('\n');

        foreach (string line in lines)
        {
            if (line == "") continue;
            
            StoreItem storeItem = GD.Load<PackedScene>("res://Scenes/StoreItem.tscn").Instantiate<StoreItem>();
            storeItem.Id = line.Trim();
            _storeItems.AddChild(storeItem);
            storeItem.UpdateUi();
        }
    }
}
