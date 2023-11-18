extends Node

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var timer

# Called when the node enters the scene tree for the first time.
func _ready():
	timer = Timer.new()
	var t = rand_range(3,5)
	add_child(timer)
	timer.set_wait_time(t)
	timer.start()
	timer.connect("timeout", self, "_on_timer_timeout")
	pass # Replace with function body.

func _on_timer_timeout():
	timer.stop()
	$Node2D/AnimationPlayer.play("ParpadeoArribaIn")
	$Node2D/AnimationPlayer2.play("ParpadeoAbajoIn")
	print ("Time is up!")
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_AnimationPlayer_animation_finished(anim_name):
	if (anim_name == "ParpadeoArribaIn"):
		$Node2D/AnimationPlayer.play("ParpadeoArribaOut")
		$Node2D/AnimationPlayer2.play("ParpadeoAbajoOut")
	#if (anim_name == "ParpadeoAbajoIn"):
		#print ("Time is up!")
	if (anim_name == "ParpadeoArribaOut"):
		var t = rand_range(3,5)
		timer.set_wait_time(t)
		timer.start()
	#if (anim_name == "ParpadeoAbajoOut"):
		#print ("Time is up!")
	pass # Replace with function body.
