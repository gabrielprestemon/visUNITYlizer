using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//http://docs.unity3d.com/ScriptReference/AudioSource.GetSpectrumData.html

public class FrequencyManager : MonoBehaviour {
	// reference for the audio source
	public AudioSource audio;
	// fft block size
	public int bufferSize = 256;
	// array for the spectrum data
	public float[] spectrum;
	// array for a modified copy of the spectrum data
	public float[] modifiedSpectrum;

	void Start()
	{
		// obtain a reference for the audio source
		audio = GetComponent<AudioSource>();
		// allocate arrays
		spectrum = new float[bufferSize];
		modifiedSpectrum = new float[bufferSize];
	}

	void Update()
	{
		// get the spectrum data from the audio source
		audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

		//// this will draw a debug line (not relevant in builds of the game)
		//for(int dbg = 1; dbg < spectrum.Length - 1; dbg++)
		//{
		//	// good distribution
		//	Debug.DrawLine(new Vector3(dbg - 1, Mathf.Log(spectrum[dbg - 1]) + 10, 2), new Vector3(dbg, Mathf.Log(spectrum[dbg]) + 10, 2), Color.cyan);
		//	// bad distribution
		//	//Debug.DrawLine(new Vector3(Mathf.Log(dbg - 1), Mathf.Log(spectrum[dbg - 1]), 3), new Vector3(Mathf.Log(dbg), Mathf.Log(spectrum[dbg]), 3), Color.yellow);
		//}

		// modify the spectrum so it's useful in this context
		ModifySpectrum();
	}

	/// <summary>
	/// Modifies the spectrum so it's useful for this program
	/// </summary>
	public void ModifySpectrum()
	{
		// modify the frequency spectrum to fit our needs
		for (int i = 0; i < spectrum.Length; i++)
		{
			// convert a linear frequency response to logorithmic
			modifiedSpectrum[i] = Mathf.Log(spectrum[i] + 10, 2) - 3f;
			// everything ends up 0.3f and some extra, so this will make the information more useable
			modifiedSpectrum[i] -= 0.321928f;
			modifiedSpectrum[i] *= 10f;
		}
	}
}
