using System.Collections.Generic;
using Godot;

public class SoundManager : Node
{
	private static SoundManager _instance = null;
	public static SoundManager GetInstance(){return _instance;}
	private List<AudioStreamPlayer> soundInstances = new List<AudioStreamPlayer>();

	[Export]
	public bool isChiptune;

	// Singleton pattern
	public override void _Ready()
	{
		if (_instance == null){
			_instance = this;
		}
		else
			QueueFree();
	}
	
	public override void _Process(float delta) {
		if(Input.IsActionJustPressed("ChangeSound")) {
			isChiptune = !isChiptune;
		}
	}

	// Spawn a sound
	public void SpawnSound(string soundName, float volumeDb = 1.0f)
	{
		if(isChiptune) soundName = "Chiptune/"+soundName+"Chiptune";
		else soundName = "Cappella/"+soundName+"Cappella";
		string soundPath = "res://Assets/Sound/"+soundName+".ogg";
		AudioStream sound = (AudioStream)GD.Load(soundPath);
		AudioStreamPlayer soundPlayer = new AudioStreamPlayer();
		AddChild(soundPlayer);
		soundPlayer.Stream = sound;
		soundPlayer.Connect("finished", this, "_OnSoundFinished", new Godot.Collections.Array { soundPlayer });
		soundPlayer.VolumeDb=volumeDb;
		soundPlayer.Play();
	}

	// Handle the finished signal to free the AudioStreamPlayer
	private void _OnSoundFinished(AudioStreamPlayer soundPlayer)
	{
		soundInstances.Remove(soundPlayer);
		soundPlayer.QueueFree();
	}
	
	public void on_play_sound() {
		SpawnSound("Button");
	}
}
