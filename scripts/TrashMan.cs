using Godot;
using System;

public partial class TrashMan : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animatedSprite;
	
	private AnimatedSprite2D _animatedSwordSprite;
	
	private String _sceneName;
	
	public CollisionShape2D _sword;
	
	public override void _Ready()
	{
		// Access to animation globally
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		// Access to sword node globally
		_sword = GetNode<CollisionShape2D>("SwordArea/CollisionShape2D");
		// Access to sword animation globally
		_animatedSwordSprite = GetNode<AnimatedSprite2D>("SwordAnimation");
		_sceneName = GetTree().CurrentScene.Name;
		_animatedSprite.Play("idle");
		_animatedSwordSprite.Play("default");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}
		// User goes left
		if(velocity.X < 0)
		{
			_animatedSprite.Play("idle");
			_animatedSprite.FlipH = true;
			_animatedSwordSprite.FlipH = true;
		}	
		// User goes right
		if(velocity.X > 0)
		{
			_animatedSprite.Play("idle");
			_animatedSprite.FlipH = false;
			_animatedSwordSprite.FlipH = false;
		}	
		// User goes up
		if(velocity.Y < 0)
		{
			_animatedSprite.Play("idleup");
			//_animatedSprite.FlipV = true;
		}
		// User goes down
		if(velocity.Y > 0)
		{
			_animatedSprite.Play("idledown");
			//_animatedSprite.FlipV = false;
		}	
		Velocity = velocity;
		MoveAndSlide();
		swing_sword();
	}
	public void swing_sword()
	{
		if (Input.IsActionJustPressed("fight"))
		{
			// Start fight animation
			_animatedSprite.Play("fight");
			// Renable sword area hitbox
			_sword.Disabled = false;
			_animatedSprite.Visible = false;
			_animatedSwordSprite.Visible = true;
		}
	}
	private void _on_animated_sprite_2d_animation_finished()
	{
		// Return to idle position
		_animatedSprite.Play("idle");
		// Hide sword hitbox
		_sword.Disabled = true;
		_animatedSprite.Visible = true;
		_animatedSwordSprite.Visible = false;
	}
}
