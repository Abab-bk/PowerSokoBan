using Godot;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class LevelItem : Panel
{
    [Export] private TextureRect _textureRect;
    [Export] private Button _button;
    [Export] private Label _levelLabel;
    
    public int Id;
    public bool Unlocked;

    public override void _Ready()
    {
        _button.Pressed += delegate
        {
            if (!Unlocked) return;
            Master.GetInstance().StopAllMusicEvent();
            Master.GetInstance().NeedLoadLevel = Id;
            SceneManager.ChangeSceneTo(Master.GetInstance().StartMenu, "res://Scenes/World.tscn");
        };
        UpdateUi();
    }

    private void UpdateUi()
    {
        _levelLabel.Text = Id.ToString();
        if (Unlocked)
        {
            _textureRect.Texture = GD.Load<Texture2D>("res://Assets/Ui/Signs/OwnedSign.png");
            return;
        }
        
        _textureRect.Texture = GD.Load<Texture2D>("res://Assets/Ui/Signs/NotOwnedSign.png");
    }
}
