using Godot;
using System;

public partial class mob : CharacterBody2D
{   
	// Constants for movement and jumping
	public const float SpeedRight = 100.0f;
	public const float SpeedLeft = -100.0f;
	public const float JumpVelocity = -400.0f;

	// Gravity value obtained from project settings
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Default attack state is standing still
	public int state = 1;

	// Used for a simple attack state machine
	public bool timer = true;
	public double time = 0;

	// Attack state, default is "wonder"
	public String attackState = "wonder";

	// Position of the player (TrashMan) when in attack state
	public Vector2 playerPos = new Vector2(0, 0);

	// Reference to AnimatedSprite2D, CharacterBody2D, and Timer nodes
	public AnimatedSprite2D _animatedSprite;
	public CharacterBody2D _trashman;
	public Timer _timer;

	// Random number generator
	Random r = new Random();

	// Exported variable for initial position
	[Export]
	public Vector2 initalPos = new Vector2(621, 540);

	// Called when the node is added to the scene for the first time
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_trashman = GetParent().GetNode<CharacterBody2D>("TrashMan");
		_timer = GetNode<Timer>("Timer");
		_animatedSprite.Play("idle");
	}

	// Called every frame. Delta is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GD.Print(_timer.TimeLeft);

		if (timer)
		{
			time += delta;
			// Change danny state every 2 seconds
			if (time > 2f)
			{
				state = r.Next(0, 4);
				time = 0;
			}
		}

		Vector2 velocity = Velocity;

		// User outside of fight area
		if (attackState == "wonder")
		{
			_timer.Stop();
			if (state == 3)
			{
				velocity.X = SpeedLeft;
			}
			if (state == 2)
			{
				velocity.X = SpeedRight;
			}
			else if (state == 1)
			{
				_animatedSprite.Play("idle");
				velocity.X = 0;
			}
			Velocity = velocity;
			MoveAndSlide();
		}
		// User entered inner area
		else if (attackState == "fight")
		{
			Position += (_trashman.Position - Position) / 40;
		}
		// User entered outer area
		else if (attackState == "interested")
		{
			Position += (_trashman.Position - Position) / 80;
		}
	}

	// Called when the mob enters the sword's interaction area
	private void _on_interaction_area_area_shape_entered(Rid area_rid, Area2D area, long area_shape_index, long local_shape_index)
	{
		if (area.Name == "SwordArea")
		{
			GD.Print("Danny ded");
			QueueFree();
		}
	}

	// Called when the mob enters the attack range
	private void _enter_attack_state(Node2D body)
	{
		if (body.Name == "TrashMan") // Ensures that other collisions don't count
		{
			_animatedSprite.Play("fight");
			attackState = "fight";
		}
	}

	// Called when the mob exits the attack range
	private void _on_attack_range_body_exited(Node2D body)
	{
		if (body.Name == "TrashMan")
		{
			GD.Print("Losing interest");
			_timer.Start(5);
		}
		// Return to random state when the user is outside the zone
	}

	// Called when the timer times out
	private void _on_timer_timeout()
	{
		attackState = "wonder";
	}

	// Called when the mob enters the interest range
	private void _on_interest_range_body_entered(Node2D body)
	{
		if (body.Name == "TrashMan") // Ensures that other collisions don't count
		{
			_timer.Stop();
			if (attackState != "fight") // Still want fast movement if the player touched inside the area
			{
				_animatedSprite.Play("fight");
				attackState = "interested";
			}
		}
	}
}
