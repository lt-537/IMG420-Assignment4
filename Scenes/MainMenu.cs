using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] public string LevelScenePath = "res://Scenes/main.tscn";
	public override void _Ready()
	{
		GetNode<Button>("VBoxContainer/Start").Pressed += () => GetTree().ChangeSceneToFile(LevelScenePath);
		
		GetNode<Button>("VBoxContainer/Quit").Pressed += () => GetTree().Quit();

	}
	
	private void _on_start_pressed()
	{
		GD.Print("[MainMenu] Start Pressed");
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}
	
	private void _on_quit_pressed()
	{
		GD.Print("[MainMenu] Quit Pressed");
		GetTree().Quit();
	}
}
