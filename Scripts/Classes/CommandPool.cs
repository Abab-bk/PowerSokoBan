using System.Collections.Generic;
using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public class CommandPool
{
    private readonly Actor _actor;
    private readonly int _maxCommands = 10;
    private readonly Stack<ICommand> _mRedoStack;
    private readonly LinkedList<ICommand> _mUndoDeque;

    public CommandPool(Actor actor)
    {
        _actor = actor;
        _mRedoStack = new Stack<ICommand>();
        _mUndoDeque = new LinkedList<ICommand>();
    }
    
    public void Register(ICommand command)
    {
        _mRedoStack.Clear();

        if (_mUndoDeque.Count >= _maxCommands)
        {
            _mUndoDeque.RemoveFirst();
        }
        
        _mUndoDeque.AddLast(command);
    }
    
    public void Undo()
    {
        if (_mUndoDeque.Count <= 0) return;
        
        ICommand command = _mUndoDeque.Last.Value;
        _mUndoDeque.RemoveLast();
        command.Undo(_actor);
        
        _mRedoStack.Push(command);
    }

    public void Redo()
    {
        if (_mRedoStack.Count <= 0) return;
        
        ICommand command = _mRedoStack.Pop();
        command.Execute(_actor);
        _mUndoDeque.AddLast(command);
    }
}