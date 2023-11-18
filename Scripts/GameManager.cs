using Godot;
using System;

	
public class GameManager : Node
{
	private readonly static GameManager _instance = new GameManager();
	
	private GameManager() {
		
	}
	
	public static GameManager Instance {
		get{
			return _instance;
		}
	}
	
	private int score;
	public bool endlessMode = false;
	public bool finished = false;
	
	[Export]
	private float totalTime = 10;
	
	private float pendingTime;
	Node sceneManager_;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GDScript SceneManager = (GDScript)GD.Load("res://Scripts/SceneManager.gd");
		score = 0;
		pendingTime = totalTime;
		
		sceneManager_ = GetTree().Root.GetNode("SceneManager");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{	
		if (!_instance.endlessMode && !finished)
		{
			pendingTime -= delta;
			
			if (pendingTime <= 0)
			{
				finished = true;
				sceneManager_.Call("onEnd");
			}
		}
  	}
	
	public void AddScore(int num) {
		score+=num;
	}
	
	public int GetScore() {
		return score;
	}
	
	public float GetTotalTime() {
		return totalTime;
	}
}
