using Godot;
using System;

public partial class Start : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Initialization logic can be added here if needed
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Currently, no continuous processing logic in this method
	}

	// Called when the button is pressed
	private void _on_pressed()
	{
		// Changes the scene to "level1.tscn" when the button is pressed
		GetTree().ChangeSceneToFile("res://levels/level1.tscn");
	}
}
