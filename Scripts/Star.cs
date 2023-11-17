using Godot;
using System;

public class Star : RigidBody2D
{
	enum StarState {
		NOT_PREPARED,
		PREPARED,
		CAUGHT,
		DEAD
	}
	
	private StarState state;
	
	[Export]
	float deathTime = 1.0f;
	
	float catchPercentage;
	
	public InputManager inputManager;
	
	private float deathCD;
	
	private bool caught;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deathCD = 0.0f;
		caught = false;
		catchPercentage = 0.5f + GD.Randf()*0.2f;
		state = StarState.NOT_PREPARED;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
	  	deathCD += delta;
		
		switch(state) {
			case StarState.NOT_PREPARED:
				if(deathCD >= deathTime*catchPercentage) {
					state = StarState.PREPARED;
					GetNode<Sprite>("Sprite").Modulate = new Color(1,1,0);
				}
				else if(Input.IsActionPressed("Catch")) {
					state = StarState.DEAD;
					GetNode<Sprite>("Sprite").Modulate = new Color(1,0,0);
				}
				break;
			case StarState.PREPARED:
				if(!caught && deathCD >= deathTime*catchPercentage && Input.IsActionPressed("Catch")) {
					caught = true;
					int baseScore = 50;
					float restPercentage = 1 - catchPercentage;
					float percentageFromTotal = deathCD/deathTime;
					//El basescore por la distancia porcentual que quedaba desde que ha sido cogida
					//hasta su muerte
					float percentageBonusScore = baseScore * (1-((percentageFromTotal-catchPercentage)/(1-catchPercentage)));
					GameManager.Instance.AddScore(baseScore + (int)percentageBonusScore);
					state = StarState.CAUGHT;
					GetNode<Sprite>("Sprite").Modulate = new Color(0,1,0);
				}
				break;
			case StarState.CAUGHT:
				//Dejar que muera
				break;
			case StarState.DEAD:
				//FadeOut del objeto hasta la muerte
				break;
		}
		
		if(deathCD >= deathTime)
			Free();
  	}
	
	public void SetInputManager(InputManager iM) {
		inputManager = iM;
	}
}
