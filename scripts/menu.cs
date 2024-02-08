using Godot;
using System;

public partial class menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Initializes the menu by grabbing focus on the "Start" button
		GetNode<Button>("VBoxContainer/Start").GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Currently, no continuous processing logic in this method
	}

	// Called when the "Start" button is pressed
	private void _on_start_pressed()
	{
		// Changes the scene to the "level1.tscn" file
		GetTree().ChangeSceneToFile("res://levels/level1.tscn");
	}

	// Called when the "Quit" button is pressed
	private void _on_quit_pressed()
	{
		// Quits the game
		GetTree().Quit();
	}
}
