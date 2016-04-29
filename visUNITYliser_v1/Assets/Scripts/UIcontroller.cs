using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour {

	// reference for the canvas transform rect
	public RectTransform canvasRect;
	// reference for the control panel transform rect
	public RectTransform controlPanel;
	// reference for the control panel background
	public Image controlPanelImage;

	// reference for the library pulldown menu game object
	public GameObject library;
	public GameObject inputField;

	public GameObject sceneselect;

	// playback UI object references
	public GameObject playpause;
	public GameObject skipforward;
	public GameObject skipbackward;
	public GameObject openaudiofile;
	public GameObject loadfilepath;
	public GameObject playhead;

	// audio UI object references
	public GameObject gain_Bass;
	public GameObject gain_Middle;
	public GameObject gain_Treble;
	public GameObject frequency_Bass;
	public GameObject frequency_Middle;
	public GameObject frequency_Treble;
	public GameObject reset;

	// visual UI object references
	public GameObject sizeIntensity;
	public GameObject updateIntensity;
	public GameObject colorSpeed;
	public GameObject colorIntensity;
	public GameObject baseColor;
	public GameObject rotationSpeed;
	public GameObject lightOrbitSpeed;


	// index for radio button (selection grid) selection
	public int selectedControls = 0;
	public string[] radioControlTexts = { "Visual", "Audio", "Hide" };

	// Use this for initialization
	void Start () {
		PinControls();
	}

	/// <summary>
	/// This will handle the radio button displays and interactions
	/// </summary>
	void OnGUI()
	{
		GUILayout.BeginVertical("Box");
		selectedControls = GUILayout.SelectionGrid(selectedControls, radioControlTexts, 3);
		ChangeControlPanel(selectedControls);
		GUILayout.EndVertical();
	}

	/// <summary>
	/// Pins all the controls to the screen dimensions
	/// </summary>
	public void PinControls()
	{
		// set up the control panel position
		controlPanel.transform.position = new Vector3((canvasRect.transform.position.x - (canvasRect.rect.width * 2f / 5f)), canvasRect.transform.position.y + (canvasRect.rect.height * 21f / 100f), -1);

		sceneselect.transform.position = new Vector3(canvasRect.rect.width * 19f / 20f, /*canvasRect.rect.height - */65f, 0);

		// pin all the controls
		PinPlaybackControls();
		PinVisualControls();
		PinEQControls();
	}

	/// <summary>
	/// Pins the playback controls to the screen dimensions
	/// </summary>
	void PinPlaybackControls()
	{
		// 100 pixels = 1 unit
		//  vector3(x, y, z):
		//  x = width multiplier
		//  y = height multiplier
		//  z = nothing (always 0)
		
		// set up playhead position                           middle x                    1/15 from bottom y
		playhead.transform.position = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height * 1f / 15f, 0);
		// set up playhead scale                                    4/5 screen width      standard height
		playhead.transform.localScale = new Vector3((canvasRect.rect.width * 4f / 5f) / 700f, 1f, 0);
		// set up play button position                            middle x          2/15 from bottom y
		playpause.transform.position = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height * 2f / 15, 0);
		// set up skip forward button position                           1/15 from middle x                                 2/15 from bottom y
		skipforward.transform.position = new Vector3((canvasRect.rect.width / 2f) + (canvasRect.rect.width / 15f), canvasRect.rect.height * 2f / 15, 0);
		// set up skip backward button position                              1/15 from middle x                              2/15 from bottom y
		skipbackward.transform.position = new Vector3((canvasRect.rect.width / 2f) - (canvasRect.rect.width / 15f), canvasRect.rect.height * 2f / 15, 0);
		// set up load file path button position                                     top right corner
		loadfilepath.transform.position = new Vector3(canvasRect.rect.width * 19f / 20f, canvasRect.rect.height - 25f, 0);
		// set up open audio file button position                                     top right corner
		openaudiofile.transform.position = new Vector3(canvasRect.rect.width * 19f / 20f, canvasRect.rect.height - 65f, 0);
		// set up filepath input field position                                           top right corner
		inputField.transform.position = new Vector3(canvasRect.rect.width * 5f / 6f, canvasRect.rect.height - 25f, 0);
		// set up library selection position                                             top right corner
		library.transform.position = new Vector3(canvasRect.rect.width * 5f / 6f, canvasRect.rect.height - 65f, 0);
	}

	/// <summary>
	/// Pins the visual controls to the control panel
	/// </summary>
	void PinVisualControls()
	{
		// arrange everything on the control panel
		sizeIntensity.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y + controlPanel.rect.height * 3f / 15f, 0);
		updateIntensity.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y + controlPanel.rect.height * 2f / 15f, 0);
		baseColor.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y + controlPanel.rect.height * 1f / 15f, 0);
		colorSpeed.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 0f / 15f, 0);
		colorIntensity.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 1f / 15f, 0);
		rotationSpeed.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 2f / 15f, 0);
		lightOrbitSpeed.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 3f / 15f, 0);
	}

	/// <summary>
	/// Pins the EQ controls to the control panel
	/// </summary>
	void PinEQControls()
	{
		// arrange everything on the control panel
		frequency_Treble.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y + controlPanel.rect.height * 3f / 15f, 0);
		gain_Treble.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y + controlPanel.rect.height * 2f / 15f, 0);
		frequency_Middle.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y + controlPanel.rect.height * 1f / 15f, 0);
		gain_Middle.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 0f / 15f, 0);
		frequency_Bass.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 1f / 15f, 0);
		gain_Bass.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 2f / 15f, 0);
		reset.transform.position = new Vector3(controlPanel.transform.position.x, controlPanel.transform.position.y - controlPanel.rect.height * 3f / 15f, 0);
	}

	/// <summary>
	/// Resets the values of the EQ controls to defaults
	/// </summary>
	public void ResetEQ()
	{
		Debug.Log("reset EQ");
		// reset gains to 2
		gain_Bass.GetComponent<Slider>().value = 2f;
		gain_Middle.GetComponent<Slider>().value = 2f;
		gain_Treble.GetComponent<Slider>().value = 2f;
		// reset frequencies to appropriate center frequencies
		frequency_Bass.GetComponent<Slider>().value = 30f;
		frequency_Middle.GetComponent<Slider>().value = 600f;
		frequency_Treble.GetComponent<Slider>().value = 17000f;
	}

	/// <summary>
	/// Toggles the active state of the audio control objects
	///   (Hides or shows the audio controls based on boolean input)
	///     notHidden == true -> unhide the controls
	///     notHidden == false -> hide the controls
	/// </summary>
	/// <param name="notHidden"></param>
	public void HideAudioControls(bool notHidden)
	{
		// if the input is true, show all the controls (set active == true)
		// else the input is false, hide all the controls (set active == false)
		playpause.SetActive(notHidden);
		skipforward.SetActive(notHidden);
		skipbackward.SetActive(notHidden);
		openaudiofile.SetActive(notHidden);
		playhead.SetActive(notHidden);
	}

	/// <summary>
	/// Toggles the active state of the visual control objects
	///   (Hides or shows the visual controls based on boolean input)
	///     notHidden == true -> unhide the controls
	///     notHidden == false -> hide the controls
	/// </summary>
	/// <param name="notHidden"></param>
	public void HideVisualControls(bool notHidden)
	{
		// if the input is true, show all the controls (set active == true)
		// else the input is false, hide all the controls (set active == false)
		sizeIntensity.SetActive(notHidden);
		updateIntensity.SetActive(notHidden);
		colorSpeed.SetActive(notHidden);
		colorIntensity.SetActive(notHidden);
		baseColor.SetActive(notHidden);
		rotationSpeed.SetActive(notHidden);
		lightOrbitSpeed.SetActive(notHidden);
	}


	/// <summary>
	/// Toggles the active state of the EQ control objects
	///   (Hides or shows the EQ controls based on boolean input)
	///     notHidden == true -> unhide the controls
	///     notHidden == false -> hide the controls
	/// </summary>
	/// <param name="notHidden"></param>
	public void HideEQ(bool notHidden)
	{
		// if the input is true, show all the controls (set active == true)
		// else the input is false, hide all the controls (set active == false)
		gain_Bass.SetActive(notHidden);
		gain_Middle.SetActive(notHidden);
		gain_Treble.SetActive(notHidden);
		frequency_Bass.SetActive(notHidden);
		frequency_Middle.SetActive(notHidden);
		frequency_Treble.SetActive(notHidden);
		reset.SetActive(notHidden);
	}

	/// <summary>
	/// Sets which set of controls will be displayed on the control panel
	///  0 = EQ (audio) controls, 1 = visual controls, 2 = Hide all
	/// </summary>
	/// <param name="controlSet"></param>
	public void ChangeControlPanel(int controlSet)
	{
		switch (controlSet)
		{
			// case 0: show the visual controls
			case 0:
				controlPanelImage.enabled = true;
				HideEQ(false);
				HideVisualControls(true);
				break;
			// case 1: show the EQ controls
			case 1:
				controlPanelImage.enabled = true;
				HideEQ(true);
				HideVisualControls(false);
				break;
			// case 2: hide everything
			case 2:
				controlPanelImage.enabled = false;
				HideEQ(false);
				HideVisualControls(false);
				break;
			// default: hide everything
			default:
				controlPanelImage.enabled = false;
				HideEQ(false);
				HideVisualControls(false);
				break;
		}
	}
}
