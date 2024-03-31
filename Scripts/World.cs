using System;
using System.Collections.Generic;
using Godot;
using PowerSokoBan.Scripts.Classes;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts;

public partial class World : Node2D
{
    [Export] private LevelGenerator _levelGenerator;
    [Export] private Ui _ui;

    private const string SavePath = "user://save.dat";
    
    private readonly List<LevelDisplayInfo> _allLevels = new List<LevelDisplayInfo>();

    private int _currentLevelIndex = -1;
    private string _levelContent;

    private Stack<MapState> _mapHistory = new Stack<MapState>();
    
    public override void _Ready()
    {
        if (!LoadGame())
        {
            EnterNextLevel();
        }
        
        Master.GetInstance().UpdateUiEvent += UpdateUi;
        Master.GetInstance().EnterNextLevelEvent += EnterNextLevel;
        Master.GetInstance().ResetCurrentLevelEvent += ResetCurrentLevel;
        Master.GetInstance().SaveMapEvent = SaveMap;
        Master.GetInstance().LoadMapEvent = LoadMap;
        Master.GetInstance().GetActorByPosEvent = GetActorByPos;
    }

    private void ResetCurrentLevel()
    {
        foreach (var child in _levelGenerator.GetChildren())
        {
            child.CallDeferred("queue_free");
        }

        _mapHistory = new Stack<MapState>();
        
        LevelInfo levelInfo = _levelGenerator.ResetGenerateLevel(GetNextLevelName());
        
        if (levelInfo == null)
        {
            return;
        }

        Master.GetInstance().CanEnterNextLevel = false;
        _ui.SetLevelInfo(levelInfo);
        UpdateUi();
    }
    
    private void EnterNextLevel()
    {
        GD.Print("进入下一层");
        
        foreach (var child in _levelGenerator.GetChildren())
        {
            child.CallDeferred("queue_free");
        }
        
        _currentLevelIndex += 1;
        SaveGame();
        LevelInfo levelInfo = _levelGenerator.ResetGenerateLevel(GetNextLevelName());
        
        if (levelInfo == null)
        {
            return;
        }

        Master.GetInstance().CanEnterNextLevel = false;
        _ui.SetLevelInfo(levelInfo);
        UpdateUi();
    }
    
    private string GetNextLevelName()
    {
        if (_levelContent == null)
        {
            var file = FileAccess.Open("res://Assets/Levels/list.txt", FileAccess.ModeFlags.Read);
            _levelContent = file.GetAsText();
            file.Close();
            
            foreach (var line in _levelContent.Split("\n"))
            {
                var s = line.Split(":");
             
                if (s.Length < 1) continue;
                
                string fileName = s[0];
                string printName = s[1];
                
                LevelDisplayInfo level = new LevelDisplayInfo(fileName, printName, false);
                _allLevels.Add(level);
            }
        }

        if (_currentLevelIndex >= _allLevels.Count)
        {
            throw new IndexOutOfRangeException("没有更多关卡了");
        }

        return _allLevels[_currentLevelIndex].FileName;
    }
    
    private void UpdateUi()
    {
        _ui.UpdateUi();
    }

    private void SaveGame()
    {
        ConfigFile config = new ConfigFile();
        config.SetValue("Game", "CurrentLevelIndex", _currentLevelIndex);
        config.Save(SavePath);
    }

    private bool LoadGame()
    {
        ConfigFile config = new ConfigFile();
        Error error = config.Load(SavePath);

        if (error != Error.Ok) return false;
        
        _currentLevelIndex = (int)config.GetValue("Game", "CurrentLevelIndex", -1);
        ResetCurrentLevel();
        return true;
    }

    private void SaveMap(Vector2 playerPos)
    {
        MapState mapState = new MapState();

        mapState.PlayerPos = playerPos;

        foreach (var functionBlock in _levelGenerator.FunctionBlocks)
        {
            FunctionBlockMapInfo mapInfo = new FunctionBlockMapInfo();
            mapInfo.Id = functionBlock.Id;
            mapInfo.Pos = functionBlock.GlobalPosition;
            mapInfo.BlockHidden = !functionBlock.Visible;
            mapState.FunctionBlockMapInfos.Add(mapInfo);
        }
        
        _mapHistory.Push(mapState);
    }

    private Actor GetActorByPos(Vector2 pos)
    {
        foreach (Actor actor in GetTree().GetNodesInGroup("Actors"))
        {
            if (actor.GlobalPosition == pos)
            {
                return actor;
            }
        }

        return null;
    }

    private void LoadMap(Player player)
    {
        if (_mapHistory.Count <= 0)
        {
            return;
        }

        MapState mapState = _mapHistory.Pop();

        player.GlobalPosition = mapState.PlayerPos;

        foreach (var functionBlock in _levelGenerator.FunctionBlocks)
        {
            foreach (var functionBlockMapInfo in mapState.FunctionBlockMapInfos)
            {
                if (functionBlockMapInfo.Id != functionBlock.Id)
                {
                    continue;
                }
                functionBlock.LoadFunctionBlockMapInfo(functionBlockMapInfo, player);
            }
        }
    }
}

class MapState
{
    public Vector2 PlayerPos;
    public List<FunctionBlockMapInfo> FunctionBlockMapInfos = new List<FunctionBlockMapInfo>();
}

public class FunctionBlockMapInfo
{
    public Vector2 Pos;
    public int Id;
    public bool BlockHidden;
}

class LevelDisplayInfo
{
    public readonly string FileName;
    public string PrintName;
    public bool Completion;

    public LevelDisplayInfo(string fileName, string printName, bool completion)
    {
        FileName = fileName;
        PrintName = printName;
        Completion = completion;
    }
}