using Godot;
using System;

public partial class Menu : Control
{
	public void _on_play_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/game.tscn");
	}

	public void _on_options_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/options.tscn");
	}

	public void _on_exit_pressed(){
		GetTree().Quit();
	}
}
