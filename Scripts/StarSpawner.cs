using Godot;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;

public class StarSpawner : Area2D
{
	[Export]
	float spawnTimeMax = 5;
	[Export]
	float spawnTimeMin = 3;	
	[Export]
	float finalSpawnTimeMax = 2;
	[Export]
	float finalSpawnTimeMin = 1;
	
	private string spawnSound = "StarFall";
	
	private float maxTimeReduction;
	private float minTimeReduction;
	
	[Export]
	float starLife = 1.0f;
	[Export]
	float rocketLife = 1.5f;
	
	private List<Star> starList;
	
	[Export]
	public PackedScene starScene { get; set; }
	
	private float spawnTime;
	private float spawnCD;
	private float gameTime;

	private bool captureStarJustPressed = false;

	[Export]
	public Node TargetNode { get; set; } = null;

	[Export]
	private Sprite cityLights;
	
	float lerpFactor = 2.0f;
	Color lightOffColor = new Color(1,1,1,1);
	Color lightsFullColor = new Color(10,10,1,1);
	float starInteractionStep = 2.5f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cityLights = GetTree().Root.GetNode<Sprite>("SceneManager/CurrentScene/Control2/CanvasLayer/CityBackgroundLights");
		bool aux = (bool)GetTree().Root.GetNode("SceneManager").Get("endless");

		Position = new Vector2(GetViewport().Size.x*0.8f,0);
	
		spawnCD = 0.0f;
		spawnTime = GD.Randi() % (spawnTimeMax+1-spawnTimeMin) + spawnTimeMin;
		starList = new List<Star>();
		
		GameManager.Instance.endlessMode = aux;
		gameTime = GameManager.Instance.GetTotalTime();
		
		maxTimeReduction = (finalSpawnTimeMax - spawnTimeMax)/gameTime;
		minTimeReduction = (finalSpawnTimeMin - spawnTimeMin)/gameTime;

		captureStarJustPressed = false;
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
		captureStarJustPressed = Input.IsActionJustPressed("Catch");

	  	spawnCD+=delta;
		spawnTimeMax += (maxTimeReduction*delta)/gameTime;
		spawnTimeMin += (minTimeReduction*delta)/gameTime;
		if(spawnCD>=spawnTime) {
			spawnTime = GD.Randi() % (spawnTimeMax+1-spawnTimeMin) + spawnTimeMin;
			spawnCD = 0.0f;
			SpawnStar();
		}

		Color lightsColor = cityLights.SelfModulate;
		lightsColor = lightsColor.LinearInterpolate(lightOffColor, lerpFactor*delta);
		cityLights.SelfModulate = lightsColor;
		//GD.Print(lightsColor);
  	}
	
	private void SpawnStar() {
		spawnTime = GD.Randi() % (spawnTimeMax+1-spawnTimeMin) + spawnTimeMin;
			
		Star star = starScene.Instance<Star>();
		star.SetStarSpawner(this);
		starList.Insert(starList.Count, star);
		AddChild(star);
		GiveRandomPosDir(ref star);
		SoundManager.GetInstance()?.SpawnSound(spawnSound);
		star.Connect("PositionSignal", GetNode("/root/SceneManager/CurrentScene/Control2/Node2D/CanvasLayer2/ManoGato"), "_on_PositionSignal");
	}
	
	private void GiveRandomPosDir(ref Star star) {
		Vector2 direction = new Vector2(0,0);
		int randomX = 0;
		int randomY = 0;
		
		float control1Distance = 0.0f, control2Distance = 0.0f;
		float curve1Strength = 0.0f, curve2Strength = 0.0f;
		//Estrella fugaz
		if(GD.Randi()%2 == 0) {
			bool fromRight = (GD.Randi()%2 == 0);
			star.SetDeathTime(starLife);
			randomX = (int)(GD.Randi() % 50);
			if(fromRight) randomX = (int)(GD.Randi() % 50)+(int)(Position.x);
			randomY = 0;
			star.GlobalPosition = new Vector2(0, 0);
			
			int xDir = -1;
			if(fromRight) xDir = 1;
			direction = new Vector2((GD.Randf()+0.5f)*(-1*xDir), GD.Randf()+0.2f);
			spawnSound = "StarFall";
		}
		//Cobete
		else {
			star.SetDeathTime(rocketLife);
			randomX = (int)(GD.Randi() % GetViewport().Size.x);
			randomY = (int)(GetViewport().Size.y);
			star.GlobalPosition = new Vector2(0, 0);
			
			direction = new Vector2(0, -1);
			
			control1Distance = GD.Randf()*0.25f;
			control2Distance = GD.Randf()*0.25f;
			curve1Strength = GD.Randf()*0.5f;
			curve2Strength = GD.Randf()*0.5f;
			spawnSound = "Firework";
		}
		
		direction = direction.Normalized();		
		Vector2 startPoint = new Vector2(randomX, randomY);
		Vector2 endPoint = startPoint + (direction*GetViewport().Size.y);
		star.CreatePath(startPoint, endPoint, control1Distance, curve1Strength, control2Distance, curve2Strength, 20, 2f);
	}
	
	public void DeathOfAStar(Star star) {
		if(starList.IndexOf(star)>=0)
			starList.RemoveAt(starList.IndexOf(star));
	}
	
	public bool IsStarFront(Star star) {
		return starList.IndexOf(star) == 0;
	}
	
	public bool ShouldGetInput(Star star) {
		int starPos = starList.IndexOf(star);
		for(int i = 0; i < starPos; i++) {
			if(starList.ElementAt(i).GetState() == Star.StarState.PREPARED) {
				return false;
			}
		}
		//Solo contamos esto un único frame
		if(captureStarJustPressed){
			captureStarJustPressed=false;
			return true;
		}
		
		return false;
	}

	public void StarInteractionDone(Star.StarState resultState){
		Color lColor = cityLights.SelfModulate;
		switch(resultState){
			case Star.StarState.PREPARED:
			lColor = new Color(10,10,1,1);
			//lColor.r += starInteractionStep;
			//lColor.g += starInteractionStep;
			break;
			case Star.StarState.CAUGHT:
			lColor = new Color(1,10,1,1);
			//lColor.g += starInteractionStep;
			break;
			case Star.StarState.DEAD:
			lColor = new Color(10,1,1,1);
			//lColor.r += starInteractionStep;
			break;
			case Star.StarState.NOT_PREPARED:
			break;
		}
		//lColor.a += starInteractionStep;
		cityLights.SelfModulate = lColor;
	}
}
