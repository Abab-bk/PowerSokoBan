using Godot;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public class Master
{
    private static Master _instance;
    public Actor.Direction PlayerLastDirection { get; set; }
    
    public delegate void EnterNextLevelEventHandler();
    public delegate void RedoCommandEventHandler();
    public delegate void UndoCommandEventHandler();
    public delegate void UpdateUiEventHandler();
    public delegate void ResetCurrentLevelEventHandler();
    
    public RedoCommandEventHandler RedoCommandEvent = () => { };
    public UndoCommandEventHandler UndoCommandEvent = () => { };
    public EnterNextLevelEventHandler EnterNextLevelEvent = () => { };
    public UpdateUiEventHandler UpdateUiEvent = () => { };
    public ResetCurrentLevelEventHandler ResetCurrentLevelEvent = () => { };
    
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