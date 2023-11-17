extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_TextureButton2_button_down():
	FreeStylers()
	pass
	

func _on_TextureButton_button_down():
	GameJam()
	pass # Replace with function body.


func _on_TextureButton3_button_down():
	Twitter()
	pass # Replace with function body.

func FreeStylers():
	OS.shell_open("https://freestylers-studio.itch.io/")
	pass # Replace with function body.

func GameJam():
	OS.shell_open("https://itch.io/jam/mermelada-jam")
	pass # Replace with function body.
	
func Twitter():
	OS.shell_open("https://twitter.com/FreeStylers_Dev")
	pass # Replace with function body.
