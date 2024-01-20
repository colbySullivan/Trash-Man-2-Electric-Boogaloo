using Godot;
using System;

public partial class TileMap : Godot.TileMap
{
	public FastNoiseLite moisture = new FastNoiseLite();
	public FastNoiseLite temperature = new FastNoiseLite();
	public FastNoiseLite altitude = new FastNoiseLite();
	public int width = 128;
	public int height = 128;
	private CharacterBody2D player { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		moisture.Seed = 10;
		temperature.Seed = 10;
		altitude.Seed = 10;
		altitude.Frequency = 0.005f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.player = this.GetNode<CharacterBody2D>("TrashMan");
		generate_chunk(player.Position);
		
	}
	
	public void generate_chunk(Vector2 position)
	{
		var tile_pos = LocalToMap(position);
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				var moist = moisture.GetNoise2D(tile_pos.X-width/2 + x, tile_pos.Y-height/2 + y)*10;
				var temp = temperature.GetNoise2D(tile_pos.X-width/2 + x, tile_pos.Y-height/2 + y)*10;
				var alt = altitude.GetNoise2D(tile_pos.X-width/2 + x, tile_pos.Y-height/2 + y)*10;
				SetCell(0, new Vector2I((tile_pos.X-64)+x, (tile_pos.Y-64)+y), 0, new Vector2I((int)(moist+10)/5, (int)(temp+10)/5));
			}
		}
	}
}
