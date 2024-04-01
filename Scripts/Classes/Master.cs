﻿using Godot;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public class Master
{
    private static Master _instance;
    public Actor.Direction PlayerLastDirection { get; set; }
    public Player Player;

    public delegate void EnterNextLevelEventHandler(int value);
    public delegate void UpdateUiEventHandler();
    public delegate void ResetCurrentLevelEventHandler();
    public delegate void SaveMapEventHandler(Vector2 playerPos);
    public delegate void LoadMapEventHandler(Player player);

    public delegate Actor GetActorByPosHandler(Vector2 position);
    
    public EnterNextLevelEventHandler EnterNextLevelEvent;
    public UpdateUiEventHandler UpdateUiEvent = () => { };
    public ResetCurrentLevelEventHandler ResetCurrentLevelEvent = () => { };
    public SaveMapEventHandler SaveMapEvent;
    public LoadMapEventHandler LoadMapEvent;
    public GetActorByPosHandler GetActorByPosEvent;

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