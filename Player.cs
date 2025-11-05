using Godot;

/// <summary>
/// A basic player controller for a 2D game.  This script demonstrates simple
/// physics (movement, gravity and jumping), collision handling via CharacterBody2D
/// and playing idle/walk animations on an AnimatedSprite2D child.  To use this
/// script, attach it to a CharacterBody2D node in your scene and add an
/// AnimatedSprite2D and CollisionShape2D as children.  Define the animations
/// "idle" and "walk" in the AnimatedSprite2D's SpriteFrames resource.
/// </summary>
public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 100f;
	// Negative value because in Godot, upward movement has a negative Y component.
	[Export] public float JumpVelocity = -200f;
	[Export] public float Gravity = 300f;
	[Export] public int MaxLives = 3;
	[Export] public NodePath LivesLabelPath;
	[Export] public float InvulnSeconds = 0.6f;

	private AnimatedSprite2D _anim;
	private int lives;
	private float _invuln = 0f;

	public override void _Ready()
	{
		// Cache a reference to the AnimatedSprite2D for switching animations and flipping
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		// lives of player
		lives = MaxLives;
		AddToGroup("player");
	
		
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_invuln > 0f)
		{
			_invuln -= (float)delta;
		}
		// Obtain the current velocity so we can modify it
		Vector2 v = Velocity;

		// Horizontal input: positive for right, negative for left
		float inputX = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		v.X = inputX * Speed;

		// Apply gravity every frame if not on the floor
		v.Y += Gravity * (float)delta;

		// Jump when the player presses the jump action (ui_accept) and the body is on the floor
		if (IsOnFloor() && Input.IsActionJustPressed("ui_accept"))
		{
			v.Y = JumpVelocity;
		}

		// Assign the modified velocity back to the CharacterBody2D and move
		Velocity = v;
		MoveAndSlide();

		// Flip and play animations based on movement
		if (_anim != null)
		{
			if (Mathf.Abs(inputX) > 0.01f)
			{
				_anim.FlipH = inputX < 0;
				_anim.Play("run");
			}
			else
			{
				_anim.Play("idle");
			}
		}
	}
	
	public void TakeDamage(int amount)
	{
		if(_invuln > 0f) return;
		lives -= amount;
		if(lives < 0) lives = 0;
		GetTree().CallGroup("game", "LoseLife", amount);
		_invuln = InvulnSeconds;		
	}
	
}
