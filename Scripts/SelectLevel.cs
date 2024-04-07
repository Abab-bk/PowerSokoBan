using Godot;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class SelectLevel : Control
{
    [Export] private GridContainer _levelItems;
    [Export] private Button _closeBtn;
    
    public override void _Ready()
    {
        _closeBtn.Pressed += Hide;
        UpdateUi();
    }

    private void UpdateUi()
    {
        foreach (Node levelItem in _levelItems.GetChildren())
        {
            levelItem.QueueFree();
        }
        
        FileAccess file = FileAccess.Open("res://Assets/Levels/list.txt", FileAccess.ModeFlags.Read);
        if (file == null) return;
        
        string text = file.GetAsText();
        file.Close();
        
        int unlockedCount = 1;
        ConfigFile configFile = new ConfigFile();
        Error error = configFile.Load("user://save.dat");
        if (error != Error.Ok)
        {
            unlockedCount = 1;
        }
        
        unlockedCount = (int)configFile.GetValue("Game", "CurrentLevelIndex", 0);
        
        string[] lines = text.Split('\n');

        int tempCount = 0;
        foreach (string line in lines)
        {
            if (line == "") continue;
            LevelItem levelItem = GD.Load<PackedScene>("res://Scenes/LevelItem.tscn").Instantiate<LevelItem>();
            levelItem.Id = tempCount;
            
            if (levelItem.Id <= unlockedCount) levelItem.Unlocked = true;
            
            _levelItems.AddChild(levelItem);
            tempCount += 1;
        }
    }
}
