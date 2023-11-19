extends Node

signal normalMode
signal endlessMode

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func NormalMode():
	emit_signal("normalMode")
	pass # Replace with function body.

func EndlessMode():
	emit_signal("endlessMode")
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


func _on_Control_normalMode():
	get_tree().root.get_node("SceneManager")._on_Control_normalMode()
	pass # Replace with function body.

func _on_Control_endlessMode():
	get_tree().root.get_node("SceneManager")._on_Control_endlessMode()
	pass # Replace with function body.
