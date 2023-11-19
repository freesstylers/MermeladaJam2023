extends Button

signal play_sound

# Called when the node enters the scene tree for the first time.
func _ready():
	connect("play_sound", get_node("/root/SceneManager/SoundManager"), "on_play_sound");
	pass # Replace with function body.

func _on_NormalMode_button_down():
	emit_signal("play_sound")
	pass # Replace with function body.

func _on_EndlessMode_button_down():
	_on_NormalMode_button_down()
	pass # Replace with function body.

func _on_Credits_button_down():
	_on_NormalMode_button_down()
	pass # Replace with function body.

func _on_HowToPlay_button_down():
	_on_NormalMode_button_down()
	pass # Replace with function body.

func _on_ChangeMusic_button_down():
	_on_NormalMode_button_down()
	pass # Replace with function body.

func _on_Back_button_down():
	_on_NormalMode_button_down()
	pass # Replace with function body.
