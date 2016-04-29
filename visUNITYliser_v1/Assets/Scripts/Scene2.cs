using UnityEngine;
using System.Collections;

public class Scene2 : Scene {

	// reference for the wall planes
	public GameObject[] backgroundFaces;
	// template for the objects that will be instantiated
	public GameObject objectTemplate;

	/// <summary>
	/// Initialize the scene and return an array of references to the objects
	/// </summary>
	/// <returns></returns>
	public override GameObject[] Initialize()
	{
		numObjects = 64;
		GameObject[] r = new GameObject[numObjects];
		// instantiate the towers
		for (int i = 0; i < r.Length; i++)
		{
			// spawn the object
			r[i] = Instantiate(objectTemplate, new Vector3(Random.Range(-49f, 49f), Random.Range(-49f, 49f), Random.Range(-49f, 49f)), new Quaternion()) as GameObject;
		}
		return r;
	}

	/// <summary>
	/// returns the target size vector based on the initial vector input.
	/// initial = initial size vector
	/// i = intensity factor
	/// f = frequency factor
	/// </summary>
	/// <param name="initial"></param>
	/// <param name="i"></param>
	/// <param name="f"></param>
	/// <returns></returns>
	public override Vector3 GetTargetSize(Vector3 initial, float i, float f)
	{
		Vector3 target = initial;
		target.x = 2f + (i * f);
		target.y = 2f + (i * f);
		target.z = 2f + (i * f);
		return target;
	}

	/// <summary>
	/// Sets the background color of the scene
	/// m = material which holds the color for the background
	/// </summary>
	/// <param name="m"></param>
	public override void SetBackgroundColor(Material m)
	{
		for(int i = 0; i < backgroundFaces.Length; i++)
		{
			backgroundFaces[i].GetComponent<Renderer>().material = m;
		}
	}

	/// <summary>
	/// Gets the background color of the scene
	/// </summary>
	/// <returns></returns>
	public override Material GetBackgroundColor()
	{
		return backgroundFaces[0].GetComponent<Renderer>().material;
	}
}
