using Godot;
using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts;

public partial class StoreItem : Panel
{
    [Export] private TextureRect _textureRect;
    [Export] private TextureRect _unlockSign;
    [Export] private Button _button;
    
    private bool _isUnlocked = false;
    
    public string Id;

    public override void _Ready()
    {
        _button.Pressed += Pressed;
        Master.GetInstance().UpdateStoreUiEvent += UpdateUi;
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
                _isUnlocked = true;
                Master.GetInstance().OpenedSkins.Add(Id);
                UpdateUi();
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
