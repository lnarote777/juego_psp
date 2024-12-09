using Godot;
using System;

public partial class Gameover : Control
{
	public void _on_start_over_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/game.tscn");
	}

	public void _on_exit_pressed(){
		GetTree().Quit();
	}
}
