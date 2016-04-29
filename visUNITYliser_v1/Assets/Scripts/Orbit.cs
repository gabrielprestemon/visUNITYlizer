using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	// speed for the rotation
	public float orbitalPeriod = 2.5f;
	// radius for the satellite
	public float radius = 10f;
	// height from orbit point for the satellite
	public float altitude = 10f;
	// the satellite which orbits
	public GameObject satellite;
	// if the satellite is a camera, include that as well
	public Camera camera;

	// Update is called once per frame
	void Update()
	{
		// rotate the orbit center point, which will move the camera along a circumference
		transform.Rotate(Vector3.up, orbitalPeriod * Time.deltaTime);
	}

	/// <summary>
	/// Set the orbital period
	/// </summary>
	/// <param name="p"></param>
	public void SetOrbitalPeriod(float p)
	{
		orbitalPeriod = p;
	}

	/// <summary>
	/// If there is a camera, set the projection to orthographic
	/// </summary>
	public void SetCameraProjectionOrthographic()
	{
		camera.orthographic = true;
	}

	/// <summary>
	/// If there is a camera, set the projection to perspective
	/// </summary>
	public void SetCameraProjectionPerspective()
	{
		camera.orthographic = false;
	}

	/// <summary>
	/// Set the position of the satellite
	/// (Usually only to modify the radius (z)
	///   and the altitude (y))
	/// </summary>
	/// <param name="p"></param>
	public void SetPosition(Vector3 p)
	{
		satellite.transform.localPosition = p;
	}
}
