extends Node

signal normalMode
signal endlessMode

# Called when the node enters the scene tree for the first time.
func _ready():
	while (get_tree().get_root().get_node("SceneManager/SoundManager").get_child_count() > 0):
		get_tree().get_root().get_node("SceneManager/SoundManager").remove_child(get_tree().get_root().get_node("SceneManager/SoundManager").get_child((0)))
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func NormalMode():
	emit_signal("normalMode")
	get_tree().get_root().get_node("SceneManager/GameManager")._instance.SetScore(0)
	pass # Replace with function body.

func EndlessMode():
	emit_signal("endlessMode")
	get_tree().get_root().get_node("SceneManager/GameManager")._instance.SetScore(0)
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

func _on_HowToPlay_button_down():
	get_node("HowToPlayContainer").visible = true
	get_node("MainMenuContainer").visible = false
	pass # Replace with function body.

func _on_HowToPlayBack_button_down():
	get_node("HowToPlayContainer").visible = false
	get_node("MainMenuContainer").visible = true
	pass # Replace with function body.


func _on_ChangeMusic_button_down():
	var aux = get_tree().get_root().get_node("SceneManager/SoundManager")._instance.changeMusic()
	
	if (aux == true):
		$MainMenuContainer/VBoxContainer/MarginContainer2/VBoxContainer/Label.text = "Chiptune"
	else:
		$MainMenuContainer/VBoxContainer/MarginContainer2/VBoxContainer/Label.text = "A Capella"
	pass # Replace with function body.
