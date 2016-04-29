using UnityEngine;
using System.Collections;

public class Scene1Manager : MonoBehaviour {

	// reference for the base plane
	public GameObject plane;
	// distance the towers will be placed from the center
	public float radius = 4.0f;
	// template for the objects that will be instantiated
	public GameObject towerTemplate;
	// number of towers to instantiate
	public int numTowers = 8;
	// an array of the objects that are instantiated
	public GameObject[] objectReferences;

	// array which holds the spectrum received from the frequency manager
	public float[] spectrum;

	// intensity factor for the tranformation of the object transforms
	public float intensity = 10f;

	// frames it takes for lerping between object transforms
	public int updateSpeed = 1;
	// counts the frames since the last transform update
	private int transformUpdateFrameCounter = 0;
	// hold the initial transform size for lerping
	private Vector3[] initialSize;
	// hold the target transform size for lerping
	private Vector3[] targetSize;

	// number of colors being morphed between
	public int numColors = 6;
	// holds the different colors to be displayed
	public Material[] colors;
	// counts the frames since the last color update
	private int colorUpdateFrameCounter = 0;
	// frames it takes for lerping between colors
	public int colorMorphSpeed = 100;
	// the ending index for color range
	public int colorMorphRangeHot = 5;
	// the starting index for color range
	public int colorMorphRangeCold = 0;
	// the index of the current color for the towers
	public int currentTowerColor = 0;
	// index of the current color for the base plane
	public float currentBaseColor = 1f;

	// individual references for the colors
	//  because the array is having trouble staying filled on compile
	public Material blue;
	public Material purple;
	public Material cyan;
	public Material yellow;
	public Material green;
	public Material red;

	// Use this for initialization
	void Start () {
		// allocate space for the arrays
		objectReferences = new GameObject[numTowers];
		initialSize = new Vector3[numTowers];
		targetSize = new Vector3[numTowers];
		colors = new Material[numColors];

		// assign the colors to their indexes in the array
		colors[0] = purple;
		colors[1] = blue;
		colors[2] = cyan;
		colors[3] = green;
		colors[4] = yellow;
		colors[5] = red;

		// instantiate the towers
		for (int i = 0; i < numTowers; i++)
		{
			// get radians around the circle
			float arcAngle = ((2.0f * Mathf.PI) / numTowers) * i;
			// use radians to find x and z location using sin and cos
			float x = transform.position.x + (radius * Mathf.Sin(arcAngle));
			float z = transform.position.y + (radius * Mathf.Cos(arcAngle));
			// spawn the object
			objectReferences[i] = Instantiate(towerTemplate, new Vector3(x, 0, z), transform.rotation) as GameObject;
			// rotate the object
			objectReferences[i].transform.LookAt(transform);
		}

		// start the base plane off as blue
		plane.GetComponent<Renderer>().material = colors[1];

	}

	// Update is called once per frame
	void Update () {
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
			spectrum = GameObject.Find("Manager").GetComponent<FrequencyManager>().modifiedSpectrum;

			// update the tower size for lerpings
			for (int i = 0; i < objectReferences.Length; i++)
			{
				// set the initial size vectors
				initialSize[i] = objectReferences[i].transform.localScale;
				// start the target size vectors at the current size
				targetSize[i] = initialSize[i];
				// modify the height of the target size based on the frequency spectrum
				targetSize[i].y = 1f + (intensity * spectrum[i * ((spectrum.Length / 16) / objectReferences.Length)]);

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
		if(colorUpdateFrameCounter >= colorMorphSpeed)
		{
			// reset the counter to 0
			colorUpdateFrameCounter = 0;
			// update the color index
			currentTowerColor = (currentTowerColor + 1) % numColors;
		}

		// lerp between the colors
		for(int i = 0; i < objectReferences.Length; i++)
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
		if(r >= 0f)
		{
			// move the cold side up in indexes
			colorMorphRangeCold = (int)r;
			colorMorphRangeHot = 5;
		}
		// if the range is on the colder side
		else if(r < 0f)
		{
			// move the hot side down in indexes
			colorMorphRangeHot = 5 + (int)r;
			colorMorphRangeCold = 0;
		}
		// update the number of colors being morphed between
		numColors = 1 + (colorMorphRangeHot - colorMorphRangeCold);
	}

	/// <summary>
	/// Set the base plane color
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
		plane.GetComponent<Renderer>().material.Lerp(colors[indexedColor], colors[nextIndex], c);
	}
}
