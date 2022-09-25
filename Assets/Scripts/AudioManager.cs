using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	// public static AudioManager instance;

	public Sound[] sounds;

	[HideInInspector]
	public bool musicPlaying;

	void Awake()
	{
		// if (instance != null)
		// {
		// 	Destroy(gameObject);
		// }
		// else
		// {
		// 	//  instance = this;
		// 	DontDestroyOnLoad(gameObject);
		// }

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
		musicPlaying = true;
	}

	public void PlaySoundEffect(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Play();
	}

	public void Play(string sound)
	{
		if (musicPlaying) {
			Sound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			s.source.Play();
		}
	}

	public void Stop(string sound) {
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	public void StopAll() {
		foreach (Sound s in sounds) {
			s.source.Stop();
		}
	}

	public void ToggleMusic() {
		if (musicPlaying) {
			musicPlaying = false;
			StopAll();
		} else {
			musicPlaying = true;
			Play("Level Theme");
		}
	}

}
