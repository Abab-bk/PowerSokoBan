using Godot;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class StoreItem : Panel
{
    [Export] private TextureRect _textureRect;
    [Export] private TextureRect _unlockSign;
    [Export] private Button _button;

    private Node _pockAd;
    
    private bool _isUnlocked = false;
    private bool _needUnlock = false;
    
    public string Id;

    public override void _Ready()
    {
        _button.Pressed += Pressed;
        Master.GetInstance().UpdateStoreUiEvent += UpdateUi;
        _pockAd = GetNode<Node>("/root/PockAD");
        _pockAd.Connect("get_reward", new Callable(this, nameof(GetReward)));
        _pockAd.Connect("reward_skip", new Callable(this, nameof(ClosedReward)));
        _pockAd.Connect("rewrad_closed", new Callable(this, nameof(ClosedReward)));
        _pockAd.Connect("reward_failed", new Callable(this, nameof(ClosedReward)));
    }

    private void ClosedReward()
    {
        if (_needUnlock == false) return;
        _needUnlock = false;
        Master.GetInstance().ShowPopupEvent(new PopupInfo(
            "\ud83d\ude1e",
            "\ud83c\ude1a\ud83c\udfc6\u27a1\ud83c\ude1a\ud83d\udc40\ud83d\udcfa",
            false,
            delegate { },
            delegate { }
        ));
    }
    
    private void GetReward()
    {
        if (_needUnlock == false) return;
        
        Master.GetInstance().ShowPopupEvent(new PopupInfo(
            "\u270c",
            "\ud83c\udfc6\ud83c\udfc6\ud83c\udfc6",
            false,
            delegate { },
            delegate { }
        ));
        
        _needUnlock = false;
        _isUnlocked = true;
        Master.GetInstance().OpenedSkins.Add(Id);
        UpdateUi();
        Master.GetInstance().SaveSkinFile();
    }

    private void Pressed()
    {
        Master.GetInstance().PlaySoundEvent(Audios.Click);
        if (_isUnlocked)
        {
            Master.GetInstance().ShowPopupEvent(new PopupInfo(
                    "\ud83d\udcb1\u2754",
                    "\ud83d\udc4c\u2754",
                    true,
                    delegate
                    {
                        GD.Print("更换 " + Id + " 皮肤");
                        Master.GetInstance().CurrentSkin = Id;
                        Master.GetInstance().CurrentSkinChangedEvent();
                        Master.GetInstance().UpdateStoreUiEvent();
                    },

                    delegate
                    {
                        GD.Print("取消更换");
                    }

                )
            );
            return;
        }

        Master.GetInstance().ShowPopupEvent(new PopupInfo(
            "\ud83d\uded2",
            "\ud83d\udc40\ud83d\udcc0\u2754",
            true,
            delegate
            {
                GD.Print("购买 " + Id + " 皮肤");
                _needUnlock = true;
                _pockAd.Call("show_reward_video_ad");
            },

            delegate
            {
                GD.Print("取消购买");
            }
            )
        );
    }

    public void UpdateUi()
    {
        _textureRect.Texture = GD.Load<Texture2D>($"res://Assets/Textures/Skins/{Id}.png");
        
        if (Master.GetInstance().OpenedSkins.Contains(Id))
        {
            _unlockSign.Texture = GD.Load<Texture2D>("res://Assets/Ui/Signs/OwnedSign.png");
            _isUnlocked = true;
            
            if (Master.GetInstance().CurrentSkin == Id)
            {
                _unlockSign.Texture = GD.Load<Texture2D>("res://Assets/Ui/Signs/CurrentSign.png");
            }
            
            return;
        }
        
        _isUnlocked = false;
        _unlockSign.Texture = GD.Load<Texture2D>("res://Assets/Ui/Signs/NotOwnedSign.png");
    }
}
