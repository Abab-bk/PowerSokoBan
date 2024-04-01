using Godot;

namespace PowerSokoBan.Scripts;

public partial class StartCut : Control
{
    [Export] private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer.AnimationFinished += delegate
        {
            SceneManager.ChangeSceneTo(this, "res://Scenes/StartMenu.tscn");
        };
        
        _animationPlayer.Play("run");
    }
}
