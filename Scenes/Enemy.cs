using Godot;

public partial class Enemy : CharacterBody2D
{
	// movement
	[Export] public float Speed = 70f;
	[Export] public float Accel = 900f;
	[Export] public float Gravity = 1400f;
	[Export] public float MaxFallSpeed = 1800f;

	// target & child paths 
	[Export] public NodePath TargetPath; // Player (CharacterBody2D)
	[Export] public NodePath AnimatedSpritePath = "AnimatedSprite2D";
	[Export] public NodePath NavigationAgentPath = "NavigationAgent2D";

	// anim names
	[Export] public string WalkAnimation = "walk";
	[Export] public string IdleAnimation = "idle";

	// agent tuning
	[Export] public float PathDesiredDistance = 10f;
	[Export] public float TargetDesiredDistance = 4f;

	// damage
	[Export] public int Damage = 1;
	[Export] public float AttackCooldown = 0.6f;
	[Export] public float AttackRange = 22f;
	

	private NavigationAgent2D _navAgent;
	private CharacterBody2D _target;
	private AnimatedSprite2D _anim;

	private float _cooldownTimer = 0f;
	private float _pulse = 0f;
	private float _attackTimer = 0f;


	public override void _Ready()
	{
		_navAgent = GetNodeOrNull<NavigationAgent2D>(NavigationAgentPath);
		_anim = GetNodeOrNull<AnimatedSprite2D>(AnimatedSpritePath);

		if (_navAgent != null)
		{
			_navAgent.PathDesiredDistance = PathDesiredDistance;
			_navAgent.TargetDesiredDistance = TargetDesiredDistance;
			_navAgent.AvoidanceEnabled = false;
		}

		if (TargetPath != null && !TargetPath.IsEmpty)
			_target = GetNodeOrNull<CharacterBody2D>(TargetPath);

		if (_target == null)
		{
			var players = GetTree().GetNodesInGroup("player");
			if (players.Count > 0 && players[0] is CharacterBody2D p)
			_target = p;
		}

		// Platformer body setup
		MotionMode = MotionModeEnum.Grounded;
		FloorSnapLength = 8f;

		}

	public override void _PhysicsProcess(double delta)
	{
		if (_cooldownTimer > 0f) 
			_cooldownTimer -= (float)delta;

		// Gravity
		if (!IsOnFloor())
			Velocity = new Vector2(Velocity.X, Mathf.Min(Velocity.Y + Gravity * (float)delta, MaxFallSpeed));
		else if (Velocity.Y > 0)
			Velocity = new Vector2(Velocity.X, 0);

		if (_target == null || _navAgent == null)
		{
			Animate();
			MoveAndSlide();
			return;
		}

		_navAgent.TargetPosition = _target.GlobalPosition;
		Vector2 desiredDir = Vector2.Zero;

		if (!_navAgent.IsNavigationFinished())
		{
			Vector2 next = _navAgent.GetNextPathPosition();
			Vector2 toNext = next - GlobalPosition;

			float dx = next.X - GlobalPosition.X;
			if (Mathf.Abs(dx) > 2f)
			{
				desiredDir = new Vector2(Mathf.Sign(dx), 0f);
			}
		}

		if (desiredDir == Vector2.Zero)
		{
			Vector2 toPlayer = _target.GlobalPosition - GlobalPosition;
			float dx = toPlayer.X;
			if (Mathf.Abs(dx) > 2f)
				desiredDir = new Vector2(Mathf.Sign(dx), 0f);
		}

		float targetVX = desiredDir.X * Speed;
		float newVX = Mathf.MoveToward(Velocity.X, targetVX, Accel * (float)delta);
		Velocity = new Vector2(newVX, Velocity.Y);

		MoveAndSlide();
		Animate();
		
		// melee range and cooldown
		_attackTimer -= (float)delta;
		
		if(_target != null && IsInstanceValid(_target))
		{
			float dist = GlobalPosition.DistanceTo(_target.GlobalPosition);
			if(dist <= AttackRange && _attackTimer <= 0f)
			{
				// call player damage
				_target.CallDeferred("TakeDamage", Damage);
				_attackTimer = AttackCooldown;
			}
		}
	}

	private void Animate()
	{
		if (_anim == null) return;
		bool moving = Mathf.Abs(Velocity.X) > 2f;
		if (moving)
		{
			_anim.FlipH = Velocity.X < 0f;
			if (_anim.Animation != WalkAnimation) _anim.Play(WalkAnimation);
		}	
		else
		{
			if (_anim.Animation != IdleAnimation) _anim.Play(IdleAnimation);
		}
	}

	public void OnHurtboxBodyEntered(Node body)
	{
		if (_cooldownTimer > 0f) return;
		if (body.IsInGroup("player"))
		{
			body.CallDeferred("TakeDamage", Damage);
			_cooldownTimer = AttackCooldown;
		}
	}
}
