using Godot;
using System;

public class StarSpawner : Area2D
{
	[Export]
	public int spawnTimeMax;
	[Export]
	public int spawnTimeMin;
	
	
	
	private float spawnTime;
	private float spawnCD;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnTime = GD.Randi() % (spawnTimeMax+1) + spawnTimeMin;
		GD.Print(spawnTime);
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
	  	spawnCD+=delta;
		if(spawnCD>=spawnTime) {
			GD.Print("Spawn");
			spawnCD = 0.0f;
			spawnTime = GD.Randi() % (spawnTimeMax+1) + spawnTimeMin;
			
			
			
		}
  	}
}
