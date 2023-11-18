extends Control

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

# Called when the node enters the scene tree for the first time.
func _ready():
	var aux = get_tree().get_root().get_node("SceneManager/GameManager")._instance.GetScore()
	$MainMenuContainer/VBoxContainer/Label3.text = str(aux).pad_decimals(0)
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _on_BackToMainMenu_button_down():
	get_tree().get_root().get_node("SceneManager").toMainMenu()
	pass # Replace with function body.
