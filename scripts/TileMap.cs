using Godot;
using System;

public partial class TileMap : Godot.TileMap
{
	// FastNoiseLite instances for generating procedural terrain
	public FastNoiseLite moisture = new FastNoiseLite();
	public FastNoiseLite temperature = new FastNoiseLite();
	public FastNoiseLite altitude = new FastNoiseLite();

	// Dimensions of the chunk
	public int width = 128;
	public int height = 128;

	// Reference to the player character
	private CharacterBody2D player { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Seed initialization for procedural generation
		moisture.Seed = 10;
		temperature.Seed = 10;
		altitude.Seed = 10;

		// Adjusting frequency for altitude noise
		altitude.Frequency = 0.005f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Retrieve the player character
		this.player = this.GetNode<CharacterBody2D>("TrashMan");

		// Generate the terrain chunk around the player's position
		generate_chunk(player.Position);
	}

	// Method to generate terrain chunk based on player's position
	public void generate_chunk(Vector2 position)
	{
		// Convert player's position to tile coordinates
		var tile_pos = LocalToMap(position);

		// Iterate through each tile in the chunk
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				// Generate noise values for moisture, temperature, and altitude
				var moist = moisture.GetNoise2D(tile_pos.X - width / 2 + x, tile_pos.Y - height / 2 + y) * 10;
				var temp = temperature.GetNoise2D(tile_pos.X - width / 2 + x, tile_pos.Y - height / 2 + y) * 10;
				var alt = altitude.GetNoise2D(tile_pos.X - width / 2 + x, tile_pos.Y - height / 2 + y) * 10;

				// Set the cell on the TileMap based on noise values
				SetCell(0, new Vector2I((tile_pos.X - width / 2) + x, (tile_pos.Y - height / 2) + y),
						0, new Vector2I((int)(moist + 10) / 5, (int)(temp + 10) / 5));
			}
		}
	}
}
