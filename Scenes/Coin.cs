using Godot;

public partial class Coin : Area2D
{
	[Export] public int Value = 1;
	[Export] public string FxScenePath = "res://Scenes/coin_pickup.tscn";
	
	public override void _Ready()
	{
		Monitoring = true;
		Monitorable = true;
		BodyEntered += OnBodyEntered;
		AddToGroup("coin");
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if(!body.IsInGroup("player"))
		{
			return;
		}
		GetTree().CallGroup("game", "AddCoin", Value);
		var fxPacked = ResourceLoader.Load<PackedScene>(FxScenePath);
		if(fxPacked != null)
		{
			var fx = fxPacked.Instantiate<GpuParticles2D>();
			fx.GlobalPosition = GlobalPosition;
			GetTree().CurrentScene.AddChild(fx);
			fx.Emitting = true;
			fx.Finished += () => fx.QueueFree();
		}
		QueueFree();
	}
}
