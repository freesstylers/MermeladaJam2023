using Godot;
using System;
using System.Net;

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
	float deathTime = 10.0f;

	[Export]
	bool debugPath = true;
	
	float catchPercentage;
	
	public StarSpawner starManager=null;
	
	private float deathCD;	
	private bool caught;
	
	private float opacityDeathTime;
	private float opacityDeathCD;

	CPUParticles2D trailParticles;
	CPUParticles2D explosionParticles;
	CPUParticles2D explosionReadyParticles;
	Sprite starSprite;

	Path2D starPath;
	PathFollow2D pathFollower;

	float followPathSpeed = 1.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deathCD = 0.0f;
		caught = false;
		catchPercentage = 0.2f + GD.Randf()*0.2f;
		state = StarState.NOT_PREPARED;
		
		opacityDeathTime = 0.0f;
		opacityDeathCD = 0.0f;


		starPath = GetNode<Path2D>("StarPath");
		pathFollower = GetNode<PathFollow2D>("StarPath/StarPathFollower");
		//Visuals
		starSprite = GetNode<Sprite>("StarPath/StarPathFollower/StarSprite");
		trailParticles = GetNode<CPUParticles2D>("StarPath/StarPathFollower/StarSprite/TrailParticles");
		explosionParticles = GetNode<CPUParticles2D>("StarPath/StarPathFollower/StarSprite/ExplotionParticles");
		explosionReadyParticles = GetNode<CPUParticles2D>("StarPath/StarPathFollower/StarSprite/ExplotionReadyParticles");

		// Vector2 startPoint = new Vector2(-250,100);
		// Vector2 endPoint = new Vector2(250,-100);
		// CreatePath(startPoint, endPoint, 0.25f, -0.5f, 0.25f, 0.5f, 20, followPathSpeed);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta)
  	{
	  	deathCD += delta;
		trailParticles.Direction = (LinearVelocity.Normalized())*-1;
		switch(state) {
			case StarState.NOT_PREPARED:
				if(deathCD >= deathTime*catchPercentage) {
					state = StarState.PREPARED;
					AdjustStarColors(new Color(1,1,0));
					explosionReadyParticles.Emitting = true;
					opacityDeathTime = (deathTime-deathCD)/4;
				}
				else if(GotInput()) {
					state = StarState.DEAD;
					AdjustStarColors(new Color(1,0,0));
					opacityDeathTime = (deathTime-deathCD)/4;
					starManager.DeathOfAStar();
				}
				break;
			case StarState.PREPARED:
				opacityDeathCD+=delta;
				AdjustStarColors(new Color(1,1,0, opacityDeathTime/opacityDeathCD));
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
					AdjustStarColors(new Color(0,1,0));
					opacityDeathTime = (deathTime-deathCD)/4;
					starManager.DeathOfAStar();
					explosionParticles.Emitting = true;
				}
				break;
			case StarState.CAUGHT:
				//FadeOut del objeto hasta la muerte
				opacityDeathCD+=delta;
				AdjustStarColors(new Color(0,1,0, opacityDeathTime/opacityDeathCD));
				deathTime*=2;
				break;
			case StarState.DEAD:
				//FadeOut del objeto hasta la muerte
				opacityDeathCD+=delta;
				AdjustStarColors(new Color(1,0,0, opacityDeathTime/opacityDeathCD));
				deathTime*=2;
				break;
		}
		
		if(deathCD >= deathTime) {
			starManager.DeathOfAStar();
			Free();
			return;
		}
			
		pathFollower.UnitOffset += delta * followPathSpeed; 
		//Has it finished its path?
		if (pathFollower.UnitOffset > 1.0f){
			pathFollower.UnitOffset = 0.0f;
			//starManager?.DeathOfAStar();
			//Free();
		}
  	}
	
	public StarState GetState() {	return state;	}
	public void SetStarSpawner(StarSpawner sM) {	starManager = sM;	}

	private bool GotInput() {
		return Input.IsActionPressed("Catch") && starManager.ShouldGetInput(this);
	}
	
	public void SetDeathTime(float time) {
		deathTime = time;
	}

	private void AdjustStarColors(Color newColor){
		starSprite.Modulate = newColor;
		trailParticles.Color = newColor;
		explosionParticles.Color = newColor;
	}

	//Symmetrical path
	private void CreateSymetricalPath(Vector2 startPoint, Vector2 endPoint,float controlDistanceToOpposite, float curveStrength, int numSegments, float pathPercentageDonePerSecond){
		CreatePath(startPoint, endPoint,controlDistanceToOpposite, curveStrength, controlDistanceToOpposite, curveStrength, numSegments, pathPercentageDonePerSecond );
	}

	private void CreatePath(Vector2 startPoint, Vector2 endPoint,float control1DistanceToOpposite, float curve1Strength,float control2DistanceToOpposite, float curve2Strength, int numSegments, float pathPercentageDonePerSecond){
		Curve2D curve= new Curve2D();
		float vectorMagnitude = (endPoint - startPoint).Length();
		Vector2 controlPoint1 = startPoint + ((endPoint - startPoint)*control1DistanceToOpposite);
		Vector2 controlPoint2 = endPoint + ((startPoint - endPoint)*control2DistanceToOpposite);

		Vector2 perpendicular = new Vector2(-(endPoint - startPoint).y, (endPoint - startPoint).x).Normalized();
		controlPoint1 += perpendicular*vectorMagnitude*curve1Strength;
		controlPoint2 += perpendicular*vectorMagnitude*curve2Strength;
		VisualizePointInSpace(controlPoint1, 0.1f);
		VisualizePointInSpace(controlPoint2, 0.1f);

		for (int i = 0; i <= numSegments; i++)
		{
			float u = i / (float)numSegments;
			float oneMinusU = 1 - u;

			float coefficient1 = oneMinusU * oneMinusU * oneMinusU;
			float coefficient2 = 3 * oneMinusU * oneMinusU * u;
			float coefficient3 = 3 * oneMinusU * u * u;
			float coefficient4 = u * u * u;

			Vector2 point = coefficient1 * startPoint +
							coefficient2 * controlPoint1 +
							coefficient3 * controlPoint2 +
							coefficient4 * endPoint;

			VisualizePointInSpace(point, 0.05f);
			curve.AddPoint(point);
			GD.Print("Punto curva: "+point);
		}
		starPath.Curve = curve;
		pathFollower.UnitOffset=0.0f;
		followPathSpeed = pathPercentageDonePerSecond;
	}

	public void VisualizePointInSpace(Vector2 position, float scale){
		Sprite pointSprite = new Sprite();
		AddChild(pointSprite);
		pointSprite.Texture = GD.Load<Texture>("res://icon.png"); // Change the path to your image
		pointSprite.Position = position;
		pointSprite.Scale = new Vector2(scale,scale);
	}
}
