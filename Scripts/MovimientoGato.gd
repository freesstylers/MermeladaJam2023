extends Sprite

var startPosition;
var endPosition;

var direction;
var movePerSecond;

var moving;
var movingCD;
var comingBack;
var stopped;

enum PawState {GO, STOP, BACK, REST}

export var moveTime : float
export var stopTime : float

var pawState

func _ready():
	startPosition = position;
	movingCD = 0.0;
	comingBack = false;
	stopped = false;
	pawState = PawState.REST;
	pass # Replace with function body.
	
func _process(delta):
	movingCD+=delta;
	
	match pawState:
		PawState.REST:
			pass
		PawState.GO:
			position += direction*(movePerSecond*delta);
			if movingCD>=moveTime:
				pawState = PawState.STOP;
				movingCD = 0.0;
		PawState.STOP:
			if movingCD>=stopTime:
				pawState = PawState.BACK;
				movingCD = 0.0;
		PawState.BACK:
			position += -direction*(movePerSecond*delta);
			if movingCD>=moveTime:
				pawState = PawState.REST;
	
	pass

func _on_PositionSignal(pos):
	# Aqui es la movida del lerp
	endPosition = pos;
	position = startPosition;
	direction = (endPosition - startPosition).normalized();
	var pathMagnitude = (endPosition - startPosition).length();
	movePerSecond = pathMagnitude/moveTime;
	movingCD = 0.0;
	pawState = PawState.GO;
	
	pass
