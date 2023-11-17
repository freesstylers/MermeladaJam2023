using Godot;
using System;

public class StarSpawner : Area2D
{
	[Export]
	int spawnTimeMax = 5;
	[Export]
	int spawnTimeMin = 3;
	[Export]
	float starSpeed = 150.0f;
	
	private InputManager inputManager;
	
	[Export]
	public PackedScene starScene { get; set; }
	
	private CollisionShape2D spawnArea;
	
	private float spawnTime;
	private float spawnCD;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnCD = 0.0f;
		spawnTime = GD.Randi() % (spawnTimeMax+1-spawnTimeMin) + spawnTimeMin;
		GD.Print(spawnTime);
		spawnArea = GetNode<CollisionShape2D>("CollisionShape2D");
		
		inputManager = GetTree().Root.GetNode("Node2D").GetNode("InputManager") as InputManager;
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
	  	spawnCD+=delta;
		if(spawnCD>=spawnTime) {
			GD.Print("Spawn");
			spawnCD = 0.0f;
			SpawnStar();
		}
  	}
	
	private void SpawnStar() {
		spawnTime = GD.Randi() % (spawnTimeMax+1-spawnTimeMin) + spawnTimeMin;
			
		Star star = starScene.Instance<Star>();
		star.SetInputManager(inputManager);
		AddChild(star);
		int randomX = (int)(GD.Randi() % 100);
		GD.Print(randomX);
		int randomY = (int)(GD.Randi() % 100);
		GD.Print(randomY);
		star.Position = new Vector2(randomX, randomY);
			
		Vector2 direction = new Vector2(-(GD.Randf()+0.5f), GD.Randf()+0.2f);
		direction = direction.Normalized();
			
		star.LinearVelocity = direction * starSpeed;
	}
}
