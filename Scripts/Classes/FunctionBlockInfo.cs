using PowerSokoBan.Scripts.Enums;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public class FunctionBlockInfo
{
    public readonly int FunctionBlockValue;
    public readonly FunctionBlockType FunctionBlockType;
    public Actor.Direction Direction;
    public FunctionBlock FunctionBlock;
    
    public FunctionBlockInfo(int functionBlockValue, FunctionBlockType functionBlockType, Actor.Direction direction)
    {
        FunctionBlockValue = functionBlockValue;
        FunctionBlockType = functionBlockType;
        Direction = direction;
    }

    public void SetDirection(Actor.Direction direction)
    {
        Direction = direction;
    }
}