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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
	  
  	}
	
	public void AddScore(int num) {
		score+=num;
		GD.Print(num);
	}
}
