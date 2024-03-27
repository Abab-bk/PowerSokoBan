using Godot;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public class Master
{
    private static Master _instance;
    public Actor.Direction PlayerLastDirection { get; set; }
    
    public delegate void EnterNextLevelEventHandler();
    public EnterNextLevelEventHandler EnterNextLevelEvent = () => { };
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