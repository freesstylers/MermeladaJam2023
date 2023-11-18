using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class StarSpawner : Area2D
{
	[Export]
	float spawnTimeMax = 5;
	[Export]
	float spawnTimeMin = 3;
	[Export]
	float starSpeed = 150.0f;
	
	private List<Star> starList;
	
	[Export]
	public PackedScene starScene { get; set; }
	
	private float spawnTime;
	private float spawnCD;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetPosition(new Vector2(GetViewport().Size.x*0.8f,0));
		
		spawnCD = 0.0f;
		spawnTime = GD.Randi() % (spawnTimeMax+1-spawnTimeMin) + spawnTimeMin;
		starList = new List<Star>();
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
	  	spawnCD+=delta;
		if(spawnCD>=spawnTime) {
			spawnCD = 0.0f;
			SpawnStar();
		}
  	}
	
	private void SpawnStar() {
		spawnTime = GD.Randi() % (spawnTimeMax+1-spawnTimeMin) + spawnTimeMin;
			
		Star star = starScene.Instance<Star>();
		star.SetStarSpawner(this);
		starList.Insert(starList.Count, star);
		AddChild(star);
		int randomX = (int)(GD.Randi() % 50);
		int randomY = (int)(GD.Randi() % 50);
		star.Position = new Vector2(randomX, randomY);
			
		Vector2 direction = new Vector2(-(GD.Randf()+0.5f), GD.Randf()+0.2f);
		direction = direction.Normalized();
			
		star.LinearVelocity = direction * starSpeed;
	}
	
	public void DeathOfAStar() {
		if(starList.Count > 0) {
			starList.RemoveAt(0);
		}
	}
	
	public bool IsStarFront(Star star) {
		return starList.IndexOf(star) == 0;
	}
	
	public bool ShouldGetInput(Star star) {
		int starPos = starList.IndexOf(star);
		
		for(int i = 0; i < starPos; i++) {
			if(starList.ElementAt(i).GetState() == Star.StarState.PREPARED) {
				GD.Print("Habia otra antes");
				return false;
			}
		}
		
		return true;
	}
}
