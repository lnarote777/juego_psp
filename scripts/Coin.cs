using Godot;
using System;

public partial class Coin : Area2D
{
    public void _on_body_entered(Node2D body){
        if (body.IsInGroup("player")){
            GameManager.AddCoin(); // Suma una moneda al puntaje
            QueueFree(); // Elimina la moneda de la escena
    
        }
    }
}
