using Godot;
using System;

public partial class Killzone : Area2D
{
    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");

    }
    
    void _on_body_entered(Node2D body){
        if (body is Wizard) // Asegúrate de que el cuerpo que entra es el personaje
        {
            GD.Print("You died");
            body.Call("Dead"); // Llama al método Dead del personaje
            if (_timer.IsInsideTree())
            {
                _timer.Start();
            }else
            {
                GD.Print("Timer is not in the scene tree!");
            }// Inicia el Timer
        }
    }

    void _on_timer_timeout()
    {
    }
}
