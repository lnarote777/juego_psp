using Godot;
using System;

public partial class Pause : Control
{
	[Export] public Control pause;

	public override void _Ready()
    {
        if (pause != null)
        {
            pause.Visible = false;
        }
    }

	public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("pausar"))
        {
            GD.Print("Pausar");
			if (pause != null)
			{
				pause.Visible = !pause.Visible;
				GD.Print("Pausa Visible: " + pause.Visible);
			}
        }
    }

	public void _on_resume_pressed(){
		GD.Print("Resume pressed");
		GetTree().Paused = false;
		if (pause != null)
		{
			pause.Visible = false;
		}
	}

	public void _on_options_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/options.tscn");
	}

	public void _on_exit_pressed(){
		GetTree().Quit();
	}
	
}
