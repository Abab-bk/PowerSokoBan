using Godot;

namespace PowerSokoBan.Scripts;

public class SceneManager
{
    public static void ChangeSceneTo(Node node, string scenePath)
    {
        Node loadedNode = GD.Load<PackedScene>(scenePath).Instantiate();

        if (node is Control control)
        {
            control.Hide();
        } else if (node is Node2D node2D)
        {
            node2D.Hide();
        }

        node.GetTree().Root.CallDeferred("add_child", loadedNode);
    }
}