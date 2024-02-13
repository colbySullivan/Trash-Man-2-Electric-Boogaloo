using Godot;
using System;
// https://github.com/nathanhoad/godot_dialogue_manager?tab=readme-ov-file
using DialogueManagerRuntime;

public partial class TrashMan : CharacterBody2D
{
	// Constants for movement and jumping
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// References to AnimatedSprite2D nodes
	private AnimatedSprite2D _animatedSprite;
	private AnimatedSprite2D _animatedSwordSprite;

	// Reference to the current scene name
	private String _sceneName;

	// Reference to the sword hitbox
	public CollisionShape2D _sword;

	// Called when the node enters the scene tree for the first time.
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

	// Called every physics frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Load dialogue file
		var dialogue = GD.Load<Resource>("res://dialogue/main.dialogue");
		// Start dialogue tree
		if (Input.IsActionJustPressed("Yap"))
			DialogueManager.ShowExampleDialogueBalloon(dialogue, "start");
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

		// Handle animations based on player's movement direction
		HandleAnimations(velocity);

		Velocity = velocity;
		MoveAndSlide();
		SwingSword();
	}

	// Method to handle player animations based on movement direction
	private void HandleAnimations(Vector2 velocity)
	{
		// User goes left
		if (velocity.X < 0)
		{
			_animatedSprite.Play("idle");
			_animatedSwordSprite.Play("default");
			_animatedSprite.FlipH = true;
			_animatedSwordSprite.FlipH = true;
		}
		// User goes right
		if (velocity.X > 0)
		{
			_animatedSprite.Play("idle");
			_animatedSwordSprite.Play("default");
			_animatedSprite.FlipH = false;
			_animatedSwordSprite.FlipH = false;
		}
		// User goes up
		if (velocity.Y < 0)
		{
			_animatedSwordSprite.Play("up");
			_animatedSprite.Play("idleup");
		}
		// User goes down
		if (velocity.Y > 0)
		{
			_animatedSwordSprite.Play("down");
			_animatedSprite.Play("idledown");
		}
	}

	// Method to handle swinging the sword
	private void SwingSword()
	{
		if (Input.IsActionJustPressed("fight"))
		{
			// Start fight animation
			_animatedSwordSprite.Stop();
			_animatedSwordSprite.Play();
			_sword.Disabled = false;
			_animatedSprite.Visible = false;
			_animatedSwordSprite.Visible = true;
		}
	}

	// Called when the sword animation finishes
	public void _on_sword_animation_animation_finished()
	{
		// Return to idle position
		_sword.Disabled = true;
		_animatedSprite.Visible = true;
		_animatedSwordSprite.Visible = false;
	}
}
