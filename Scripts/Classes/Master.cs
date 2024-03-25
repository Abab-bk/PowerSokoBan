using PowerSokoBan.Scripts.Prefabs;

namespace PowerSokoBan.Scripts.Classes;

public class Master
{
    private static Master _instance;
    public Player Player;
    
    private Master()
    {
    }

    public static Master getInstance()
    {
        if (_instance == null)
        {
            _instance = new Master();
        }
        return _instance;
    }
}