extends Node

const GameplayScene = preload("res://Escenas/MainScene.tscn")
const ScoreScreenScene = preload("res://Escenas/ScoreScreen.tscn")
const MainMenuScene = preload("res://Escenas/MainMenu.tscn")
var endless = false
signal size_changed
# Declare member variables here. Examples:
# var a = 2
# var b = "text"

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _on_CanvasLayer_transitioned():
	$CurrentScene.get_child(0).queue_free()
	$CurrentScene.add_child(GameplayScene.instance())
	pass # Replace with function body.

func _on_Control_normalMode():
	endless = false
	get_tree().get_root().get_node("SceneManager/GameManager")._instance.playing = true
	get_tree().get_root().get_node("SceneManager/GameManager")._instance.finished = false
	get_tree().get_root().get_node("SceneManager/GameManager")._instance.ResetPendingTime()
	$CanvasLayer.transition()
	pass # Replace with function body.

func _on_Control_endlessMode():
	endless = true
	$CanvasLayer.transition()
	pass # Replace with function body.

func onEnd():
	$CurrentScene.get_child(0).queue_free()
	$CurrentScene.add_child(ScoreScreenScene.instance())
	pass
	
func toMainMenu():
	$CurrentScene.get_child(0).queue_free()
	$CurrentScene.add_child(MainMenuScene.instance())
	pass
