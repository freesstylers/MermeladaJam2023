using Godot;
using System;

public class Star : RigidBody2D
{
	[Export]
	float deathTime = 1.0f;
	
	public InputManager inputManager;
	
	private float deathCD;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deathCD = 0.0f;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
	  	deathCD += delta;
		if(deathCD >= deathTime)
			Free();
  	}
	
	public void SetInputManager(InputManager iM) {
		inputManager = iM;
	}
}
