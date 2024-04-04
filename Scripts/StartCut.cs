using Godot;

namespace PowerSokoBan.Scripts;

public partial class StartCut : Control
{
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private TextureButton _loginBtn;

    private Node _taptap;
    
    public override void _Ready()
    {
        _taptap = GetNode<Node>("/root/Tap");
        _taptap.Connect("logined", new Callable(this, nameof(LoginDone)));
        _taptap.Connect("anti_pass", new Callable(this, nameof(AntiAddictionPass)));
        
        _animationPlayer.AnimationFinished += delegate
        {
            GD.Print(_taptap);
            if (OS.GetName() != "Android")
            {
                SceneManager.ChangeSceneTo(this, "res://Scenes/StartMenu.tscn");
            }

            _taptap.Call("is_login");
        };
        
        _animationPlayer.Play("run");

        _loginBtn.Pressed += delegate
        {
            _taptap.Call("login");
        };
    }

    private void AntiAddictionPass()
    {
        SceneManager.ChangeSceneTo(this, "res://Scenes/StartMenu.tscn");
    }

    private void LoginDone()
    {
        _taptap.Call("init_tap_anti");
        CheckAntiAddiction();
    }


    private void CheckAntiAddiction()
    {
        _taptap.Call("quick_anti");
    }
}
