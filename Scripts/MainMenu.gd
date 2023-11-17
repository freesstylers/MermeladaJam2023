extends Node

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func NormalMode():
	get_tree().change_scene("res://Escenas/MainScene.tscn")
	pass # Replace with function body.

func EndlessMode():
	pass # Replace with function body.

func Credits():
	get_node("CreditsContainer").visible = true
	get_node("MainMenuContainer").visible = false
	pass # Replace with function body.
	
func CreditsBack():
	get_node("CreditsContainer").visible = false
	get_node("MainMenuContainer").visible = true
	pass # Replace with function body.

func FreeStylers():
	OS.shell_open("https://freestylers-studio.itch.io/")
	pass # Replace with function body.

func Jam():
	OS.shell_open("https://itch.io/jam/mermelada-jam")
	pass # Replace with function body.

func Twitter():
	OS.shell_open("https://twitter.com/FreeStylers_Dev")
	pass # Replace with function body.
