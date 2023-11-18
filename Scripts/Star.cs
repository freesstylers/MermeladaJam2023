using Godot;
using System;

public class Star : RigidBody2D
{
	public enum StarState {
		NOT_PREPARED,
		PREPARED,
		CAUGHT,
		DEAD
	}
	
	private StarState state;
	
	[Export]
	float deathTime = 1.0f;
	
	float catchPercentage;
	
	public StarSpawner starManager;
	
	private float deathCD;
	
	private bool caught;
	
	private float opacityDeathTime;
	private float opacityDeathCD;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deathCD = 0.0f;
		caught = false;
		catchPercentage = 0.5f + GD.Randf()*0.2f;
		state = StarState.NOT_PREPARED;
		
		opacityDeathTime = 0.0f;
		opacityDeathCD = 0.0f;
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
				else if(GotInput()) {
					state = StarState.DEAD;
					GetNode<Sprite>("Sprite").Modulate = new Color(1,0,0);
					opacityDeathTime = (deathTime-deathCD)/4;
				}
				break;
			case StarState.PREPARED:
				if(!caught && deathCD >= deathTime*catchPercentage && GotInput()) {
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
					opacityDeathTime = (deathTime-deathCD)/4;
				}
				break;
			case StarState.CAUGHT:
				//FadeOut del objeto hasta la muerte
				opacityDeathCD+=delta;
				GetNode<Sprite>("Sprite").Modulate = new Color(0,1,0, opacityDeathTime/opacityDeathCD);
				break;
			case StarState.DEAD:
				//FadeOut del objeto hasta la muerte
				opacityDeathCD+=delta;
				GetNode<Sprite>("Sprite").Modulate = new Color(1,0,0, opacityDeathTime/opacityDeathCD);
				break;
		}
		
		if(deathCD >= deathTime) {
			starManager.DeathOfAStar();
			Free();
		}
  	}
	
	private bool GotInput() {
		return Input.IsActionPressed("Catch") && starManager.ShouldGetInput(this);
	}
	
	public void SetStarSpawner(StarSpawner sM) {
		starManager = sM;
	}
	
	public StarState GetState() {
		return state;
	}
}
