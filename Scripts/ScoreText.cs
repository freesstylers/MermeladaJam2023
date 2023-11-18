using Godot;
using System;

public class ScoreText : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = "0";
	}
	
	public override void _Process(float delta)
  	{
	  	Text = GameManager.Instance.GetScore().ToString();
  	}
}
