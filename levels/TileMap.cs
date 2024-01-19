using Godot;
using System;

public partial class TileMap : Godot.TileMap
{
	public FastNoiseLite moisture = new FastNoiseLite();
	public FastNoiseLite temperature = new FastNoiseLite();
	public FastNoiseLite altitude = new FastNoiseLite();
	public const int width = 128;
	public const int height = 128;
	
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
		CharacterBody2D player = (CharacterBody2D)GetNode("TrashMan");
		generate_chunk(player.Position);
	}
	
	public void generate_chunk(Vector2 position)
	{
		var tile_pos = LocalToMap(position);
		for(int x = 0; x < width; x++){
			for(int y = 0; x < height; x++){
				var moist = moisture.GetNoise2D(tile_pos.X-width/2 + x, tile_pos.Y-height/2 + y)*10;
				var temp = temperature.GetNoise2D(tile_pos.X-width/2 + x, tile_pos.Y-height/2 + y)*10;
				var alt = altitude.GetNoise2D(tile_pos.X-width/2 + x, tile_pos.Y-height/2 + y)*10;
				Vector2I vec; 
				vec = new Vector2I((int)tile_pos.X-width/2 + x, (int)tile_pos.Y-height/2 + y);
				Vector2I vec2;
				vec2 = new Vector2I((int)Math.Round((moist+10)/5), (int)Math.Round((temp+10)/5));
				SetCell(0, vec, 0, vec2);
			}
		}
	}
}
