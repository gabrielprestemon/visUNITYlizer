using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Equilizer : MonoBehaviour {
	public AudioMixer mixer;

	public void LowFreqUpdate(float f)
	{
		mixer.SetFloat("LowFreq", f);
	}

	public void LowGainUpdate(float g)
	{
		mixer.SetFloat("LowGain", ScaleGain(g));
	}

	public void MidFreqUpdate(float f)
	{
		mixer.SetFloat("MidFreq", f);
	}

	public void MidGainUpdate(float g)
	{
		mixer.SetFloat("MidGain", ScaleGain(g));
	}

	public void HighFreqUpdate(float f)
	{
		mixer.SetFloat("HighFreq", f);
	}

	public void HighGainUpdate(float g)
	{
		mixer.SetFloat("HighGain", ScaleGain(g));
	}

	/// <summary>
	/// Scale the gain to match the EQ component parameters
	/// </summary>
	/// <param name="g"></param>
	/// <returns></returns>
	float ScaleGain(float g)
	{
		if (g > 2)
		{
			g = (((g - 2f) * 2f) + 1f);
		}
		else if (g == 2)
		{
			g = 1f;
		}
		else if (g < 2)
		{
			g /= 2f;
		}
		return g;
	}

	/// <summary>
	/// Resets the EQ to default values
	/// </summary>
	public void ResetEQ()
	{
		LowFreqUpdate(30);
		LowGainUpdate(2);
		MidFreqUpdate(600);
		MidGainUpdate(2);
		HighFreqUpdate(17000);
		HighGainUpdate(2);
	}
}
