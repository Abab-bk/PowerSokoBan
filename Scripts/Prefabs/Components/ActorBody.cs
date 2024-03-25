using Godot;
using System;

namespace PowerSokoBan.Scripts.Prefabs.Components;

[GlobalClass]
public partial class ActorBody : StaticBody2D
{
    [Export] public Actor Actor { set; get; }
}
