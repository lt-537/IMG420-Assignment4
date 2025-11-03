using Godot;

public partial class Game : Node
{
	[Export] public NodePath CoinsLabelPath;
	[Export] public NodePath LivesLabelPath;
	[Export] public string WinScenePath = "res://Scenes/WinScreen.tscn";
	[Export] public string LoseScenePath = "res://Scenes/LoseScreen.tscn";
	[Export] public int StartingLives = 3;
	
	private int _totalCoins;
	private int _coinsCollected;
	private int _lives;
	private Label _coinsLabel;
	private Label _livesLabel;
	
	public override void _Ready()
	{
		AddToGroup("game");
		_coinsLabel = GetNodeOrNull<Label>(CoinsLabelPath);
		_livesLabel = GetNodeOrNull<Label>(LivesLabelPath);
		
		_totalCoins = GetTree().GetNodesInGroup("coin").Count;
		_coinsCollected = 0;
		_lives = StartingLives;
		UpdateUI();
	}
	
	private void UpdateUI()
	{
		if (_coinsLabel != null)
		{
			_coinsLabel.Text = $"Coins: {_coinsCollected} / {_totalCoins}";
		}
		if (_livesLabel != null)
		{
			_livesLabel.Text = $"Lives: {_lives}";
		}
	}
	
	public void AddCoin(int amount = 1)
	{
		_coinsCollected += amount;
		UpdateUI();
		if(_coinsCollected >= _totalCoins && _totalCoins > 0)
		{
			GetTree().ChangeSceneToFile(WinScenePath);
		}
	}
	
	public void LoseLife(int amount = 1)
	{
		_lives -= amount;
		if (_lives < 0) _lives = 0;
		UpdateUI();

		if (_lives <= 0)
		{
			GetTree().ChangeSceneToFile(LoseScenePath);
		}
	}
	
}
