using System;
using System.Collections.Generic;
using Godot;
using PowerSokoBan.Scripts.Classes;
namespace PowerSokoBan.Scripts;

public partial class World : Node2D
{
    [Export] private LevelGenerator _levelGenerator;
    [Export] private Ui _ui;

    private const string SavePath = "user://save.dat";
    
    private readonly List<LevelDisplayInfo> _allLevels = new List<LevelDisplayInfo>();

    private int _currentLevelIndex = -1;
    private string _levelContent;
    
    public override void _Ready()
    {
        if (!LoadGame())
        {
            EnterNextLevel();
        }
        
        Master.GetInstance().UpdateUiEvent += UpdateUi;
        Master.GetInstance().EnterNextLevelEvent += EnterNextLevel;
        Master.GetInstance().ResetCurrentLevelEvent += ResetCurrentLevel;
    }

    private void ResetCurrentLevel()
    {
        foreach (var child in _levelGenerator.GetChildren())
        {
            child.CallDeferred("queue_free");
        }
        
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