using Godot;
using System;

public partial class Gameover : Control
{
	public void _on_start_over_pressed(){
		GameManager.ResetGame();
		GetTree().ChangeSceneToFile("res://scenes/menu.tscn");

		GD.Print("Escena cambiada a game.tscn y HUD reinicializado.");
	}

	public void _on_exit_pressed(){
		GetTree().Quit();
	}
}
