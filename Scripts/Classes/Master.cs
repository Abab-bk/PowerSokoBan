using Godot;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public class Master
{
    private static Master _instance;
    public Actor.Direction PlayerLastDirection { get; set; }
    public Player Player;
    public bool OpenSettingUi = false;
    public bool SawTutorial;
    
    public delegate void EnterNextLevelEventHandler(int value);
    public delegate void UpdateUiEventHandler();
    public delegate void ResetCurrentLevelEventHandler();
    public delegate void SaveMapEventHandler(Vector2 playerPos, System.Collections.Generic.Dictionary<Actor.Direction, FunctionBlockInfo> functionBlockInfos);
    public delegate void LoadMapEventHandler(Player player);
    public delegate void BackToMainMenuEventHandler();

    public delegate void PlaySoundHandler(Audios audio);

    public delegate Actor GetActorByPosHandler(Vector2 position);
    
    public EnterNextLevelEventHandler EnterNextLevelEvent;
    public UpdateUiEventHandler UpdateUiEvent = () => { };
    public ResetCurrentLevelEventHandler ResetCurrentLevelEvent = () => { };
    public SaveMapEventHandler SaveMapEvent;
    public LoadMapEventHandler LoadMapEvent;
    public GetActorByPosHandler GetActorByPosEvent;
    public PlaySoundHandler PlaySoundEvent;
    public BackToMainMenuEventHandler BackToMainMenuEvent;

    public bool CanEnterNextLevel;
    
    private Master()
    {
    }

    public static Master GetInstance()
    {
        _instance = _instance ?? new Master();
        return _instance;
    }
}