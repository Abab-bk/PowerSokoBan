using Godot;
using System;
using System.Collections.Generic;
using PowerSokoBan.Scripts.Classes.Factorys;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

[GlobalClass]
public partial class LevelGenerator : Node2D
{
    [Export]
    private TileMap _tileMap;

    public List<FunctionBlock> FunctionBlocks = new List<FunctionBlock>();
    
    const int GirdSize = 64;

    public LevelInfo ResetGenerateLevel(string fileName)
    {
        return GenerateLevel(fileName);
    }

    private LevelInfo GenerateLevel(string fileName)
    {
        _tileMap.Clear();
        FunctionBlocks.Clear();
        
        var file = FileAccess.Open("res://Assets/Levels/" + fileName + ".txt", FileAccess.ModeFlags.Read);
        if (file == null)
        {
            return null;
        }
        
        var text = file.GetAsText();
        file.Close();
        
        LevelInfo levelInfo = new LevelInfo();
        
        var lines = text.Split('\n');
        var width = lines[0].Length;
        var height = lines.Length - 1;
        int id = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (lines.Length <= y) continue;
                if (x < lines[y].Length)
                {
                    var letter = lines[y][x];
                    MapBlockInfo obj = null;
                    switch (letter)
                    {
                        case '#':
                            obj = new MapBlockInfo()
                                .SetMapBlockType(MapBlockInfo.MapBlockType.Wall);
                            break;
                        case '-':
                            obj = new MapBlockInfo()
                                .SetMapBlockType(MapBlockInfo.MapBlockType.HiddenWall);
                            break;
                        case '0':
                            // TODO: 0 是绿色，也就是箱子
                            obj = new MapBlockInfo()
                                .SetMapBlockType(MapBlockInfo.MapBlockType.FunctionBlock)
                                .SetValue(0);
                            break;
                        case '1': case '2': case '3':
                            obj = new MapBlockInfo()
                                .SetMapBlockType(MapBlockInfo.MapBlockType.FunctionBlock)
                                .SetValue(int.Parse(letter.ToString()));
                            break;
                        case 'S':
                            // TODO: S 是 Swap 也就是嘴巴
                            obj = new MapBlockInfo()
                                .SetMapBlockType(MapBlockInfo.MapBlockType.Swap);
                            break;
                        case '@':
                            obj = new MapBlockInfo()
                                .SetMapBlockType(MapBlockInfo.MapBlockType.Player);
                            break;
                        case '$':
                            obj = new MapBlockInfo()
                                .SetMapBlockType(MapBlockInfo.MapBlockType.Goal);
                            break;
                        case '.':
                            continue;
                    }

                    if (obj == null)
                    {
                        continue;
                    }

                    if (obj.GetMapBlockType() == MapBlockInfo.MapBlockType.FunctionBlock)
                    {
                        id += 1;
                        FunctionBlock functionBlock = (FunctionBlock)ActorFactory.CreateActor(ActorType.FunctionBlockRed, obj.GetValue());
                        functionBlock.LevelInfo = levelInfo;
                        levelInfo.AddTotalFunctionBlockCount(1);
                        CallDeferred("add_child", functionBlock);
                        functionBlock.CallDeferred("set_global_position", new Vector2(x, y) * GirdSize);
                        functionBlock.CallDeferred("RulePosition");

                        functionBlock.Id = id;
                        
                        FunctionBlocks.Add(functionBlock);
                        continue;
                    }

                    if (obj.GetMapBlockType() == MapBlockInfo.MapBlockType.Player)
                    {
                        Player player = (Player)ActorFactory.CreateActor(ActorType.Player);
                        
                        CallDeferred("add_child", player);
                        player.CallDeferred("set_global_position", new Vector2(x, y) * GirdSize);
                        player.CallDeferred("RulePosition");
                        
                        continue;
                    }

                    if (obj.GetMapBlockType() == MapBlockInfo.MapBlockType.Goal)
                    {
                        WinPoint winPoint = (WinPoint)ActorFactory.CreateActor(ActorType.WinPoint);
                        
                        CallDeferred("add_child", winPoint);
                        winPoint.CallDeferred("set_global_position", new Vector2(x, y) * GirdSize);
                        winPoint.CallDeferred("RulePosition");
                        
                        continue;
                    }

                    if (obj.GetMapBlockType() == MapBlockInfo.MapBlockType.HiddenWall)
                    {
                        _tileMap.SetCell(0, new Vector2I(x, y), 0);
                    }

                    // TODO: Add Swap.
                    if (obj.GetMapBlockType() == MapBlockInfo.MapBlockType.Swap)
                    {
                        FunctionBlock functionBlock = (FunctionBlock)ActorFactory.CreateActor(ActorType.FunctionBlockRed, obj.GetValue());
                        functionBlock.LevelInfo = levelInfo;
                        levelInfo.AddTotalFunctionBlockCount(1);
                        CallDeferred("add_child", functionBlock);
                        functionBlock.GlobalPosition = new Vector2I(x, y) * GirdSize;
                        functionBlock.RulePosition();
                        FunctionBlocks.Add(functionBlock);

                        continue;
                    }
                    
                    // Add Wall
                    Wall wall = (Wall)ActorFactory.CreateActor(ActorType.Wall);
                    CallDeferred("add_child", wall);
                    wall.GlobalPosition = new Vector2(x, y) * GirdSize;
                }
            }
        }

        return levelInfo;
    }
}

public class LevelInfo
{
    private int _gotFunctionBlockCount = 0;
    private int _totalFunctionBlockCount = 0;
    public int Id = 0;
    public string PrintName = "";

    public int ReduceGotFunctionBlockCount(int value)
    {
        _gotFunctionBlockCount -= value;
        return _gotFunctionBlockCount;
    }

    public int GetGotFunctionBlockCount()
    {
        return _gotFunctionBlockCount;
    }
    
    public int GetTotalFunctionBlockCount()
    {
        return _totalFunctionBlockCount;
    }
    
    public void SetGotFunctionBlockCount(int value)
    {
        _gotFunctionBlockCount = value;
        if (_gotFunctionBlockCount == _totalFunctionBlockCount)
        {
            Master.GetInstance().CanEnterNextLevel = true;
        }
    }
    
    public void AddGotFunctionBlockCount(int value)
    {
        _gotFunctionBlockCount += value;
        if (_gotFunctionBlockCount == _totalFunctionBlockCount)
        {
            Master.GetInstance().CanEnterNextLevel = true;
        }
    }

    public void SetTotalFunctionBlockCount(int value)
    {
        _totalFunctionBlockCount = value;
    }

    public void AddTotalFunctionBlockCount(int value)
    {
        _totalFunctionBlockCount += value;
    }
}

class MapBlockInfo
{
    public enum MapBlockType
    {
        Wall,
        HiddenWall,
        FunctionBlock,
        Player,
        Goal,
        Swap,
    }
    
    private MapBlockType _mapBlockType;
    private int _value;

    public MapBlockType GetMapBlockType()
    {
        return _mapBlockType;
    }
    
    public int GetValue()
    {
        return _value;
    }
    
    public MapBlockInfo SetMapBlockType(MapBlockType type)
    {
        _mapBlockType = type;
        return this;
    }

    public MapBlockInfo SetValue(int value)
    {
        _value = value;
        return this;
    }
}
