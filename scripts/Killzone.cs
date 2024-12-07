using Godot;
using System;

public partial class Killzone : Area2D
{
    Timer _timer;
    void OnReady()
    {
        _timer = new Timer();
    }
    
    void _on_body_entered(Node2D body){
        GD.Print("you died");
        _timer.Start();
    }

    void _on_timer_timeout()
    {
        GetTree().ReloadCurrentScene();
    }
}
