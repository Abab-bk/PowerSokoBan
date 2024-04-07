using Godot;

namespace PowerSokoBan.Scripts;

public class SceneManager
{
    public static void ChangeSceneTo(Node node, string scenePath)
    {
        Node loadedNode = GD.Load<PackedScene>(scenePath).Instantiate();

        if (node is Control control)
        {
            GD.Print("Hide1");
            control.Hide();
        } else if (node is Node2D node2D)
        {
            GD.Print("Hide2");
            node2D.Hide();
        }
        
        node.GetTree().Root.CallDeferred("add_child", loadedNode);
        node.CallDeferred("queue_free");
    }
}