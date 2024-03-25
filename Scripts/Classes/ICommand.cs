using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public interface ICommand
{
    void Execute(Actor actor);
    void Undo(Actor actor);
}