using Godot;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class Popup : Control
{
    [Export] private Button _closeBtn;
    [Export] private Button _yesBtn;
    [Export] private Label _titleLabel;
    [Export] private Label _messageLabel;

    public PopupInfo PopupInfo;

    public override void _Ready()
    {
        ShowPopup(PopupInfo);
    }

    private void ShowPopup(PopupInfo popupInfo)
    {
        if (popupInfo == null)
        {
            return;
        }

        _titleLabel.Text = popupInfo.Title;
        _messageLabel.Text = popupInfo.Message;
        _closeBtn.Visible = popupInfo.ShowCloseBtn;

        _yesBtn.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            if (popupInfo.YesEvent != null)
            {
                popupInfo.YesEvent.Invoke();
            }
            QueueFree();
        };
        _closeBtn.Pressed += delegate
        {
            Master.GetInstance().PlaySoundEvent(Audios.Click);
            if (popupInfo.CloseEvent != null)
            {
                popupInfo.CloseEvent.Invoke();
            }
            QueueFree();
        };
    }
}

public class PopupInfo
{
    public readonly string Title;
    public readonly string Message;
    public readonly bool ShowCloseBtn;
    
    public delegate void ReactionEventHandler();
    
    public readonly ReactionEventHandler YesEvent = delegate { };
    public readonly ReactionEventHandler CloseEvent = delegate { };

    public PopupInfo(string title, string message, bool showCloseBtn = false, ReactionEventHandler yesEvent = null,
        ReactionEventHandler closeEvent = null)
    {
        Title = title;
        Message = message;
        ShowCloseBtn = showCloseBtn;
        YesEvent = yesEvent;
        CloseEvent = closeEvent;
    }
}