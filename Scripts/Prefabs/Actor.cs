using PowerSokoBan.Scripts.Classes;

namespace PowerSokoBan.Scripts.Prefabs;

using Godot;

[GlobalClass]
public partial class Actor : Godot.Node2D
{
    public int MoveDistance = 100;
    
    private CommandPool _commandPool;

    public override void _Ready()
    {
        _commandPool = new CommandPool(this);
    }

    public void RegisterCommand(ICommand command)
    {
        _commandPool.Register(command);
    }
    
    public void Undo()
    {
        _commandPool.Undo();
    }
    
    public void Redo()
    {
        _commandPool.Redo();
    }
    
}