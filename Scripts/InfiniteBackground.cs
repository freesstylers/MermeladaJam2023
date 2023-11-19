using Godot;
using System;

public class InfiniteBackground : ParallaxBackground
{
	[Export]
	float scrollingSpeed = 100;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		SetScrollBaseOffset(GetScrollBaseOffset() + new Vector2(scrollingSpeed*delta,0));
	}
}
