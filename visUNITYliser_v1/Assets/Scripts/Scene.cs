using UnityEngine;
using System.Collections;

public abstract class Scene : MonoBehaviour {

	// number of objects to instantiate
	protected int numObjects;

	/// <summary>
	/// Initialize the scene and return an array of references to the objects
	/// </summary>
	/// <returns></returns>
	public abstract GameObject[] Initialize();

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
	public abstract Vector3 GetTargetSize(Vector3 initial, float i, float f);

	/// <summary>
	/// Sets the background color of the scene
	/// m = material which holds the color for the background
	/// </summary>
	/// <param name="m"></param>
	public abstract void SetBackgroundColor(Material m);

	/// <summary>
	/// Gets the background color of the scene
	/// </summary>
	/// <returns></returns>
	public abstract Material GetBackgroundColor();
}
