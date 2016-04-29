using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioFileManager : MonoBehaviour {

	// reference for the audio source
	public AudioSource source;
	// List for the audio clips
	private List<AudioClip> clips;
	// List for the names of all the audio clips
	//  (to be used for the dropdown menu)
	private List<string> names;
	// reference for the dropdown menu, used to select the current playing track
	public Dropdown library;
	// reference for file path input field
	public InputField pathField;

	// Use this for initialization
	void Start () {
		// get a reference to the audio source
		source = GetComponent<AudioSource>() as AudioSource;
		// create the clip list and clip names list
		clips = new List<AudioClip>();
		names = new List<string>();
	}

	/// <summary>
	/// Load an audio file at a file path
	/// </summary>
	/// <param name="fpath"></param>
	public void LoadAudioFile(string fpath)
	{
		if (fpath != "")
		{
			// Replace all the backslash, because WWW no longer works with them
			fpath = fpath.Replace('\\', '/');

			// create a file reference
			WWW fileOpener = new WWW("file:// " + fpath);
			Debug.Log("File path: \"" + fpath + "\"");

			// get the file name from the filepath string
			string fname = fpath.Substring(fpath.LastIndexOf('/') + 1, fpath.Length - (fpath.LastIndexOf('/') + 1));

			// add the information to the lists
			clips.Add(fileOpener.audioClip);
			names.Add(fname);

			// add the string options to the dropdown menu
			library.ClearOptions();
			library.AddOptions(names);

			// if this is the first clip to be loaded, load it into the audio source
			if (source.clip == null)
			{
				SetCurrentClip(0);
			}
		}
	}

	/// <summary>
	/// Open a file picker dialog
	/// Currently not working
	/// </summary>
	public void OpenFilePicker()
	{
		// obtain the filepath from the file that the user selects
		// string fpath = EditorUtility.OpenFilePanel("Select Audio File", "", "wav");

		// System.Diagnostics.Process p = new System.Diagnostics.Process();
		// p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
		// p.Start();
		// string fpath = "";

		string fpath = "";
		




		// if the cancel button was pressed, don't do anything
		if (fpath != "")
		{
			// load the audio file into unity
			LoadAudioFile(fpath);
		}
	}

	/// <summary>
	/// parses a file path, make sure it's valid, then open it
	/// </summary>
	/// <param name="fpath"></param>
	public void OpenFilePath()
	{
		// ensure it might possibly be a valid file path
		if (pathField.text.Length > 4)
		{
			// check to see if the file path includes .wav file extension
			if (pathField.text[pathField.text.Length - 1] == 'v' && pathField.text[pathField.text.Length - 2] == 'a'
				&& pathField.text[pathField.text.Length - 3] == 'w' && pathField.text[pathField.text.Length - 4] == '.')
			{
				LoadAudioFile(pathField.text);
				pathField.text = "";
			}
			// if it doesn't,  let the user know
			else
			{
				pathField.text = "error";
			}
		}
	}

	/// <summary>
	/// Set the current clip to the input index
	/// </summary>
	/// <param name="index"></param>	 
	public void SetCurrentClip(int index)
	{
		// stop the clip to reduce the risk of heart disease
		source.Stop();
		// set the current clip's time to 0 to prevent any playhead/clip mismatching which might mess things up
		source.time = 0f;
		// set the playhead to 0 to improve cholesterol
		gameObject.SendMessage("PlayheadMoved", 0f);

		// change clips
		source.clip = clips[index];
	}

	/// <summary>
	/// This is the file manager version of skip forward.
	/// Will be called via gameObject.SendMessage() from AudioController
	/// </summary>
	public void IncrementClip()
	{
		// if the last index is currently selected, go to the first index
		if(library.value == clips.Count - 1)
		{
			library.value = 0;
			SetCurrentClip(0);
		}
		// else increment to the next clip
		else
		{
			SetCurrentClip(++library.value);
		}
	}

	/// <summary>
	/// This is the file manager version of skip backward
	/// Will be called via gameObject.SendMessage() from AudioController
	/// </summary>
	public void DecrementClip()
	{
		// make sure there are clips in the library
		if(clips.Count > 0)
		{
			// if the first index is currently selected, go to the last index
			if (library.value == 0)
			{
				library.value = clips.Count - 1;
				SetCurrentClip(library.value);
			}
			// else decrement to the previous clip
			else
			{
				SetCurrentClip(--library.value);
			}
		}
	}
}