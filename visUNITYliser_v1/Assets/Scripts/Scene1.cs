using UnityEngine;
using System.Collections;

public class Scene1 : Scene {

	// reference for the base plane
	public GameObject plane;
	// distance the towers will be placed from the center
	public float radius = 4.0f;
	// template for the objects that will be instantiated
	public GameObject objectTemplate;

	/// <summary>
	/// Initialize the scene and return an array of references to the objects
	/// </summary>
	/// <returns></returns>
	public override GameObject[] Initialize()
	{
		numObjects = 8;
		GameObject[] r = new GameObject[numObjects];
		// instantiate the towers
		for (int i = 0; i < r.Length; i++)
		{
			// get radians around the circle
			float arcAngle = ((2.0f * Mathf.PI) / r.Length) * i;
			// use radians to find x and z location using sin and cos
			float x = transform.position.x + (radius * Mathf.Sin(arcAngle));
			float z = transform.position.y + (radius * Mathf.Cos(arcAngle));
			// spawn the object
			r[i] = Instantiate(objectTemplate, new Vector3(x, 0, z), transform.rotation) as GameObject;
			// rotate the object
			r[i].transform.LookAt(transform);
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
		target.y = 1f + (i * f);
		return target;
	}

	/// <summary>
	/// Sets the background color of the scene
	/// m = material which holds the color for the background
	/// </summary>
	/// <param name="m"></param>
	public override void SetBackgroundColor(Material m)
	{
		// set the plane color based on a lerp between two colors
		plane.GetComponent<Renderer>().material = m;
	}

	/// <summary>
	/// Gets the background color of the scene
	/// </summary>
	/// <returns></returns>
	public override Material GetBackgroundColor()
	{
		return plane.GetComponent<Renderer>().material;
	}
}
