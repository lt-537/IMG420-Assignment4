using Godot;
using System;

public partial class LoseScreen : Control
{
	[Export] public string MainScenePath = "res://Scenes/main.tscn";
	[Export] public string MenuScenePath = "res://Scenes/MainMenu.tscn";
	
	public override void _Ready()
	{
		GetNode<Button>("CenterContainer/VBoxContainer/Retry").Pressed += () => GetTree().ChangeSceneToFile(MainScenePath);
		
		GetNode<Button>("CenterContainer/VBoxContainer/MainMenu").Pressed += () => GetTree().ChangeSceneToFile(MenuScenePath);

	}
}
