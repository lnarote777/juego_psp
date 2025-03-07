using Godot;
using System;

public partial class Winer : Control
{
	public void _on_menu_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}

	public void _on_exit_pressed(){
		GetTree().Quit();
	}

	public void _on_save_pressed(){

	}
}
