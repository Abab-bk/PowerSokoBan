using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using PowerSokoBan.Scripts.Prefabs;
using Array = Godot.Collections.Array;

namespace PowerSokoBan.Scripts.Classes;

public class Master
{
    const string SaveDataFile = "user://skins.dat";
    
    private static Master _instance;
    public Actor.Direction PlayerLastDirection { get; set; }
    public Player Player;
    public bool OpenSettingUi = false;
    public bool SawTutorial;
    
    public Array<string> OpenedSkins = new Array<string>();
    public string CurrentSkin = "Default";
    
    public delegate void EnterNextLevelEventHandler(int value);
    public delegate void UpdateUiEventHandler();
    public delegate void ResetCurrentLevelEventHandler();
    public delegate void SaveMapEventHandler(Vector2 playerPos, System.Collections.Generic.Dictionary<Actor.Direction, FunctionBlockInfo> functionBlockInfos);
    public delegate void LoadMapEventHandler(Player player);
    public delegate void BackToMainMenuEventHandler();
    public delegate void CurrentSkinChangedEventHandler();

    public delegate void PlaySoundHandler(Audios audio);

    public delegate Actor GetActorByPosHandler(Vector2 position);
    public delegate void ShowPopupHandler(PopupInfo popupInfo);

    public delegate void UpdateStoreUiHandler();
    
    public EnterNextLevelEventHandler EnterNextLevelEvent;
    public UpdateUiEventHandler UpdateUiEvent = () => { };
    public ResetCurrentLevelEventHandler ResetCurrentLevelEvent = () => { };
    public SaveMapEventHandler SaveMapEvent;
    public LoadMapEventHandler LoadMapEvent;
    public GetActorByPosHandler GetActorByPosEvent;
    public PlaySoundHandler PlaySoundEvent;
    public BackToMainMenuEventHandler BackToMainMenuEvent;
    public ShowPopupHandler ShowPopupEvent;
    public CurrentSkinChangedEventHandler CurrentSkinChangedEvent;
    public UpdateStoreUiHandler UpdateStoreUiEvent;

    public bool CanEnterNextLevel;
    
    private Master()
    {
    }

    public void SaveSkinFile()
    {
        ConfigFile config = new ConfigFile();
        config.SetValue("Skins", "Skins", OpenedSkins);
        config.SetValue("Skins", "CurrentSkin", CurrentSkin);
        config.Save(SaveDataFile);
    }

    public bool LoadSkinFile()
    {
        ConfigFile config = new ConfigFile();
        Error error = config.Load(SaveDataFile);

        if (error != Error.Ok) return false;
        
        OpenedSkins = (Array<string>)config.GetValue("Skins", "Skins", new Array<string>());
        CurrentSkin = (string)config.GetValue("Skins", "CurrentSkin", "Default");
        
        return true;
    }
    
    public static Master GetInstance()
    {
        _instance = _instance ?? new Master();
        return _instance;
    }
}