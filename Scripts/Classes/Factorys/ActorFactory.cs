using Godot;
using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes.Factorys;

public enum ActorType
{
    None,
    FunctionBlockRed,
    FunctionBlockBlue,
    FunctionBlockGreen,
    FunctionBlockYellow,
    WinPoint,
    Player,
}

public class ActorFactory
{
    public static Actor CreateActor(ActorType actorType, int value = 0, Actor.Direction direction = Actor.Direction.Left)
    {
        switch (actorType)
        {
            case ActorType.FunctionBlockRed:
                FunctionBlock functionBlock =
                    GD.Load<PackedScene>("res://Scenes/Prefabs/FunctionBlock.tscn").Instantiate() as FunctionBlock;
                
                if (functionBlock == null)
                {
                    return null;
                }
                
                functionBlock.SetFunctionBlockValue(value);
                functionBlock.SetFunctionBlockDirection(direction);
                functionBlock.SetFunctionBlockType(FunctionBlockType.Red);
                
                return functionBlock;
            
            case ActorType.FunctionBlockBlue:
                FunctionBlock blueFunctionBlock =
                    GD.Load<PackedScene>("res://Scenes/Prefabs/FunctionBlock.tscn").Instantiate() as FunctionBlock;
                
                if (blueFunctionBlock == null)
                {
                    return null;
                }
                
                blueFunctionBlock.SetFunctionBlockValue(value);
                blueFunctionBlock.SetFunctionBlockDirection(direction);
                blueFunctionBlock.SetFunctionBlockType(FunctionBlockType.Blue);
                
                return blueFunctionBlock;
            
            case ActorType.FunctionBlockGreen:
                FunctionBlock greenFunctionBlock =
                    GD.Load<PackedScene>("res://Scenes/Prefabs/FunctionBlock.tscn").Instantiate() as FunctionBlock;
                
                if (greenFunctionBlock == null)
                {
                    return null;
                }
                
                greenFunctionBlock.SetFunctionBlockValue(value);
                greenFunctionBlock.SetFunctionBlockDirection(direction);
                greenFunctionBlock.SetFunctionBlockType(FunctionBlockType.Green);
                
                return greenFunctionBlock;
            case ActorType.FunctionBlockYellow:
                FunctionBlock yellowFunctionBlock =
                    GD.Load<PackedScene>("res://Scenes/Prefabs/FunctionBlock.tscn").Instantiate() as FunctionBlock;
                
                if (yellowFunctionBlock == null)
                {
                    return null;
                }
                
                yellowFunctionBlock.SetFunctionBlockValue(value);
                yellowFunctionBlock.SetFunctionBlockDirection(direction);
                yellowFunctionBlock.SetFunctionBlockType(FunctionBlockType.Yellow);
                
                return yellowFunctionBlock;
            case ActorType.WinPoint:
                WinPoint winPoint = GD.Load<PackedScene>("res://Scenes/Prefabs/WinPoint.tscn").Instantiate() as WinPoint;
                return winPoint;
            case ActorType.Player:
                return GD.Load<PackedScene>("res://Scenes/Prefabs/Player.tscn").Instantiate() as Player;
            default:
                return null;
        }
    }
}
