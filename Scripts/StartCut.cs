using Godot;

namespace PowerSokoBan.Scripts;

public partial class StartCut : Control
{
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private TextureButton _loginBtn;

    private Node _taptap;
    
    public override void _Ready()
    {
        _taptap = GetNode<Node>("/root/GodotTapTap");
        _taptap.Connect("onLoginResult", new Callable(this, nameof(OnLoginResult)));
        _taptap.Call("onAntiAddictionCallback", new Callable(this, nameof(OnAntiAddictionCallback)));
        
        _animationPlayer.AnimationFinished += delegate
        {
            if ((bool)_taptap.Call("isLogin"))
            {
                CheckAntiAddiction();
            }
        };
        
        _animationPlayer.Play("run");

        _loginBtn.Pressed += delegate
        {
            _taptap.Call("tap_login");
        };
    }

    private void OnLoginResult(int code, string json)
    {
        GD.Print("登录结果：", code, json);
        CheckAntiAddiction();
    }

    private void OnAntiAddictionCallback(int code)
    {
        if (code != 500) return;
        SceneManager.ChangeSceneTo(this, "res://Scenes/StartMenu.tscn");
    }
    
    private void CheckAntiAddiction()
    {
        _taptap.Call("quickCheck");
    }
}
