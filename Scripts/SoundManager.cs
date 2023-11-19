using System.Collections.Generic;
using Godot;

public class SoundManager : Node
{
	private static SoundManager _instance = null;
	public static SoundManager GetInstance(){return _instance;}
	private List<AudioStreamPlayer> soundInstances = new List<AudioStreamPlayer>();

	// Singleton pattern
	public override void _Ready()
	{
		if (_instance == null){
			_instance = this;
		}
		else
			QueueFree();
	}

	// Spawn a sound
	public void SpawnSound(string soundPath, float volumeDb = 1.0f)
	{
		AudioStream sound = (AudioStream)GD.Load(soundPath);
		AudioStreamPlayer soundPlayer = new AudioStreamPlayer();
		AddChild(soundPlayer);
		soundPlayer.Stream = sound;
		soundPlayer.Connect("finished", this, "_OnSoundFinished", new Godot.Collections.Array { soundPlayer });
		soundPlayer.VolumeDb=volumeDb;
		soundPlayer.Play();
		GD.Print("Sound Spawned: "+ soundPath+" Volume: " +soundPlayer.VolumeDb);
	}

	// Handle the finished signal to free the AudioStreamPlayer
	private void _OnSoundFinished(AudioStreamPlayer soundPlayer)
	{
		GD.Print("Sound Spawned: REMOVED");
		soundInstances.Remove(soundPlayer);
		soundPlayer.QueueFree();
	}
}
