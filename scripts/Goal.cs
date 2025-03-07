using Godot;
using System;

public partial class Goal : Area2D
{
    public void _on_body_entered(Node2D body){
        if (body.IsInGroup("player")){
            GetTree().ChangeSceneToFile("scenes/winer.tscn");
        }
    }
}
