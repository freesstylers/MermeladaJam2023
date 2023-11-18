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
	[Export]
	float starLife = 1.0f;
	[Export]
	float rocketLife = 1.5f;
	
	private List<Star> starList;
	
	[Export]
	public PackedScene starScene { get; set; }
	
	private float spawnTime;
	private float spawnCD;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position = new Vector2(GetViewport().Size.x*0.8f,0);
		
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
		GiveRandomPosDir(ref star);
	}
	
	private void GiveRandomPosDir(ref Star star) {
		Vector2 direction = new Vector2(0,0);
		//Estrella fugaz
		if(GD.Randi()%2 == 0) {
			star.SetDeathTime(starLife);
			int randomX = (int)(GD.Randi() % 50);
			int randomY = (int)(100 + GD.Randi() % 50);
			star.Position = new Vector2(randomX, randomY);
			
			direction = new Vector2(-(GD.Randf()+0.5f), GD.Randf()+0.2f);
		}
		//Cobete
		else {
			star.SetDeathTime(rocketLife);
			int randomX = (int)(GD.Randi() % GetViewport().Size.x);
			int randomY = (int)(GetViewport().Size.y);
			star.Position = new Vector2(randomX, randomY);
			
			direction = new Vector2(0, -1);
		}
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
