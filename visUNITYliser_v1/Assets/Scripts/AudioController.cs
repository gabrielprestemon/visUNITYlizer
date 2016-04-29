using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {
	// reference for the audio source
	public AudioSource source;
	// reference for the playhead slider gameobject
	public Slider playhead;

	// Use this for initialization
	void Start () {
		// get the audio source
		source = GetComponent<AudioSource>();

		// get the playhead
		playhead = GameObject.Find("Playhead").GetComponent<Slider>();
		// make sure the playhead slider was found
		if (playhead == null)
		{
			Debug.Log("Could not find the slider");
		}
	}
	
	// Update is called once per frame
	void Update () {
		// make sure the playhead is in the right spot
		UpdatePlayhead();
	}

	/// <summary>
	/// Play or pause the clip
	/// </summary>
	public void PlayPause()
	{
		// make sure there is an audio clip loaded before trying to play
		if(source.clip != null)
		{
			// check the load state of the audio clip
			//  (the clip will not play unless this has been checked first)
			if (source.clip.loadState == AudioDataLoadState.Loaded)
			{
				// if the clip is playing, pause it
				if (source.isPlaying)
				{
					source.Pause();
					Debug.Log("Pause");
				}
				// otherwise, it's paused or stopped, play it
				else
				{
					source.Play();
					Debug.Log("Play");
				}
			}
			// if for some reason the clip has not been loaded, send a message to debug
			else
			{
				Debug.Log("Still loading the clip");
			}
		}
	}

	/// <summary>
	/// Skip forward to the next song in the queue
	/// </summary>
	public void SkipForward()
	{
		gameObject.SendMessage("IncrementClip");
	}

	/// <summary>
	/// Skip backward to the beginning of the song, or the previous song in the queue
	/// </summary>
	public void SkipBackward()
	{
		// make sure the source has a clip loaded
		if (source.clip != null)
		{
			source.Stop();
			// if the song is less than a tenth second in, or out of bounds (aka NaN), go to the previous song
			if (source.time < 0.1f || source.time > source.clip.length)
			{
				gameObject.SendMessage("DecrementClip");
			}
			// else set the playback time to 0.0
			else
			{
				source.time = 0.0001f;
			}
		}
		// if the clip hasn't been loaded, try skipping anyway. What's the worst that could happen?
		else
		{
			gameObject.SendMessage("DecrementClip");
		}
	}

	/// <summary>
	/// Reset the clip to pre-played position
	/// </summary>
	public void Stop()
	{
		// make sure the source has a clip loaded
		if(source.clip != null)
		{
			// if the song is playing, stop the playing (pause)
			if (source.isPlaying)
			{
				PlayPause();
			}
			// set the time to the start of the clip
			source.time = 0.0001f;
		}
	}

	/// <summary>
	/// Sets the volume of the audio listener.
	///  Takes in a value between 0.0 and 1.0 inclusive.
	///	  Converts linear input to an exponential input.
	///    (0.2 -> 0.04, 0.8 -> 0.64)
	/// </summary>
	/// <param name="linearValue"></param>
	public void SetVolume(float linearValue)
	{
		// convert linear value to exponential value
		AudioListener.volume = linearValue * linearValue;
	}

	/// <summary>
	/// Updates the playhead's position on the playhead slider
	/// </summary>
	public void UpdatePlayhead()
	{
		// check if the clip has been loaded and that the time is within the time constraints
		//  (sometimes if the clip has been loaded but not played source.time is NaN)
		if (source.clip != null && source.time >= 0f && source.time <= source.clip.length)
		{
			playhead.value = source.time / source.clip.length;
		}
		// if there is no clip or the clip's time is out of bounds (aka NaN) set the playhead to 0
		else
		{
			playhead.value = 0f;
		}
	}

	/// <summary>
	/// Called when the playhead is moved by the user
	///  Input is a value between 0.0 and 1.0
	/// </summary>
	/// <param name="time"></param>
	public void PlayheadMoved(float time)
	{
		// check if the clip has been loaded
		if (source.clip != null)
		{
			// if playing hasn't started (i.e. previous time was 0), be sure not to accidentally use unPause in playpause()
			if (source.time == 0.0f)
			{
				source.Play();
				source.Pause();
			}
			// set the clip time
			source.time = time * source.clip.length;
		}
	}
}
