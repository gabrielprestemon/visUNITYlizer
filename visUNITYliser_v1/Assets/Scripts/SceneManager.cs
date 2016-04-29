using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
	
	// the particular scene manager script
	public Scene scene;
	// scenes
	public Scene scene1Script;
	public Scene scene2Script;
	// scene objects
	public GameObject scene1Manager;
	public GameObject scene2Manager;
	// current active scene
	public int currentScene;
	// number of available scenes
	public int numScenes = 2;

	// an array of the objects that are instantiated
	public GameObject[] objectReferences;

	// array which holds the spectrum received from the frequency manager
	public float[] spectrum;

	// intensity factor for the tranformation of the object transforms
	private float intensity = 10f;
	// frames it takes for lerping between object transforms
	private int updateSpeed = 1;
	// counts the frames since the last transform update
	private int transformUpdateFrameCounter = 0;
	// hold the initial transform size for lerping
	private Vector3[] initialSize;
	// hold the target transform size for lerping
	private Vector3[] targetSize;

	// number of colors being morphed between
	private int numColors = 6;
	// holds the different colors to be displayed
	private Material[] colors;
	// counts the frames since the last color update
	public int colorUpdateFrameCounter = 0;
	// frames it takes for lerping between colors
	public int colorMorphSpeed = 100;
	// the ending index for color range
	private int colorMorphRangeHot = 5;
	// the starting index for color range
	private int colorMorphRangeCold = 0;
	// the index of the current color for the towers
	private int currentTowerColor = 0;
	// index of the current color for the base plane
	private float currentBaseColor = 1f;

	// individual references for the colors
	//  because the array is having trouble staying filled on compile
	public Material blue;
	public Material purple;
	public Material cyan;
	public Material yellow;
	public Material green;
	public Material red;

	// Use this for initialization
	void Start()
	{
		// allocate space for the color array
		colors = new Material[numColors];

		// assign the colors to their indexes in the array
		colors[0] = purple;
		colors[1] = blue;
		colors[2] = cyan;
		colors[3] = green;
		colors[4] = yellow;
		colors[5] = red;

		// start on scene 1 (currentScene increments in changeScenes())
		currentScene = numScenes - 1;
		ChangeScenes();
	}

	// Update is called once per frame
	void Update()
	{
		// count frames since last update for each visual aspect
		transformUpdateFrameCounter++;
		colorUpdateFrameCounter++;

		// change the object transforms
		TransformObjects();

		// change the object colors
		ColorChange();
	}

	/// <summary>
	/// Updates the object transforms
	/// </summary>
	public void TransformObjects()
	{
		// if the frames since last update matches the update speed, update
		if (transformUpdateFrameCounter >= updateSpeed)
		{
			// reset the counter to 0
			transformUpdateFrameCounter = 0;

			// get the spectrum
			spectrum = GetComponent<FrequencyManager>().modifiedSpectrum;

			// update the tower size for lerpings
			for (int i = 0; i < objectReferences.Length; i++)
			{
				// set the initial size vectors
				initialSize[i] = objectReferences[i].transform.localScale;
				// get the target size
				targetSize[i] = scene.GetTargetSize(initialSize[i], intensity, spectrum[i * ((spectrum.Length / 16) / objectReferences.Length)]);

				// if the update speed is 1 frame, just move to the target size
				if (updateSpeed <= 1)
				{
					objectReferences[i].transform.localScale = targetSize[i];
				}
			}
		}
		// if the update speed is greater than one frame, lerp between initial size and target size
		if (updateSpeed > 1)
		{
			float dist = (float)transformUpdateFrameCounter / (float)updateSpeed;
			// lerp the transform
			for (int i = 0; i < objectReferences.Length; i++)
			{
				objectReferences[i].transform.localScale = Vector3.Lerp(initialSize[i], targetSize[i], dist);
			}
		}
	}

	/// <summary>
	/// Set the object transform intensity parameter
	/// </summary>
	/// <param name="i"></param>
	public void SetTransformIntensity(float i)
	{
		intensity = i;
	}

	/// <summary>
	/// Set the transform update speed parameter
	///    argument float is cast to int
	/// </summary>
	/// <param name="u"></param>
	public void SetTransformUpdateSpeed(float u)
	{
		updateSpeed = (int)u;
	}

	/// <summary>
	/// Updates the object colors
	/// </summary>
	public void ColorChange()
	{
		// if the frames since last update matches the update speed, update
		if (colorUpdateFrameCounter >= colorMorphSpeed)
		{
			// reset the counter to 0
			colorUpdateFrameCounter = 0;
			// update the color index
			currentTowerColor = (currentTowerColor + 1) % numColors;
		}

		// lerp between the colors
		for (int i = 0; i < objectReferences.Length; i++)
		{
			// lerp between the colors
			objectReferences[i].GetComponent<Renderer>().material.Lerp(colors[(colorMorphRangeCold + currentTowerColor) % colors.Length], colors[(colorMorphRangeCold + ((currentTowerColor + 1) % numColors)) % colors.Length], (float)colorUpdateFrameCounter / (float)colorMorphSpeed);
		}
	}

	/// <summary>
	/// Set the color morph speed
	///    argument float is cast to int
	/// </summary>
	/// <param name="c"></param>
	public void SetColorMorphSpeed(float c)
	{
		colorMorphSpeed = (int)c;
	}

	/// <summary>
	/// Set the color morph range
	///  argument float is cast to int
	///   algorithm allows single value to modify two limits
	/// </summary>
	/// <param name="r"></param>
	public void SetColorMorphRange(float r)
	{
		// slider -4 -3 -2 -1  0  1  2  3  4
		// cold	              0  1  2  3  4
		// hot	  1  2  3  4  5

		// if the range is on the hotter side
		if (r >= 0f)
		{
			// move the cold side up in indexes
			colorMorphRangeCold = (int)r;
			colorMorphRangeHot = 5;
		}
		// if the range is on the colder side
		else if (r < 0f)
		{
			// move the hot side down in indexes
			colorMorphRangeHot = 5 + (int)r;
			colorMorphRangeCold = 0;
		}
		// update the number of colors being morphed between
		numColors = 1 + (colorMorphRangeHot - colorMorphRangeCold);
	}

	/// <summary>
	/// Set the background color
	/// </summary>
	/// <param name="c"></param>
	public void SetBaseColor(float c)
	{
		currentBaseColor = c;
		// get the initial base color index
		int indexedColor = (int)c;
		// get the amount of lerp between the index color and the next color
		c -= indexedColor;
		// get the next base color index
		int nextIndex = (indexedColor + 1 < numColors) ? indexedColor + 1 : 0;

		// set the plane color based on a lerp between two colors
		Material m = scene.GetBackgroundColor();
		m.Lerp(colors[indexedColor], colors[nextIndex], c);
		scene.SetBackgroundColor(m);
	}

	/// <summary>
	/// Change viewable scenes based on input
	/// </summary>
	public void ChangeScenes()
	{
		// find a reference for the camera
		var camera = GameObject.Find("CameraOrbiter");

		// destroy the old objects
		for (int i = 0; i < objectReferences.Length; i++)
		{
			Destroy(objectReferences[i]);
		}
		// increment the scene
		currentScene = (currentScene + 1) % numScenes;
		// change the active viewable "scene" based on the input
		switch (currentScene)
		{
			case 0:
				// set the active scene script to scene 1
				scene = scene1Script;
				// swap the visible scene objects
				scene2Manager.SetActive(false);
				scene1Manager.SetActive(true);
				// set up the camera
				camera.SendMessage("SetPosition", new Vector3(0, 6, -9));
				camera.SendMessage("SetCameraProjectionOrthographic");
				break;
			case 1:
				// set the active scene script to scene 2
				scene = scene2Script;
				// swap the visible scene objects
				scene1Manager.SetActive(false);
				scene2Manager.SetActive(true);
				// set up the camera
				camera.SendMessage("SetPosition", new Vector3(0, 10, -40));
				camera.SendMessage("SetCameraProjectionPerspective");
				break;
			default:
				break;
		}
		// reinitialize the objects and arrays
		objectReferences = scene.Initialize();
		initialSize = new Vector3[objectReferences.Length];
		targetSize = new Vector3[objectReferences.Length];
		// prompt the backgorund to change its color
		SetBaseColor(currentBaseColor);
	}
}