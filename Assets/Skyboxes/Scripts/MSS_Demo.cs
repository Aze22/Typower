using UnityEngine;
using System.Collections;

// Standard Skybox Demo Preview Script

// Use this script to specify a number of standard skybox materials 
// in the materials array which can be cycled using input.

// Attach this script to the main camera of a scene. If not already added,
// the script will add the required Skybox component to the camera.

[RequireComponent(typeof(Skybox))]
public class MSS_Demo : MonoBehaviour {	
	// Array of normal skybox materials to cycle through in the demo
	public Material[] materials;
	
	// Counter for current index in the skybox material array	
	private int _skybox;
	
	// GUI text for displaying simple instructions
	private GUIText _guiText;
	
	void Start () {		
		// Create a GUI texture
		_guiText = new GameObject("Label").AddComponent<GUIText>();
		_guiText.transform.position = new Vector3(0.05f, 0.95f, 0);
		_guiText.fontStyle = FontStyle.Bold;
		_guiText.fontSize = 16;
		
		// Use the method to set the initial skybox material
		SetSkybox(_skybox);
		
		// Hide the mouse pointer
		Screen.showCursor = false;
	}
	
	void Update () {
		// If input left arrow / minus / fire 2...		
		if (Input.GetKeyDown(KeyCode.LeftArrow) || 
			Input.GetKeyDown(KeyCode.Minus) || 
			Input.GetKeyDown(KeyCode.KeypadMinus) || 
			Input.GetButtonDown("Fire2")) {			
			// Cycle to previous skybox material in material array
			if (_skybox-- <= 0) _skybox = materials.Length - 1;
			// Use the method to change to the new skybox material
			SetSkybox(_skybox);
		}
		
		// If input right  arrow / plus / fire 1...	
		if (Input.GetKeyDown(KeyCode.Space) || 
			Input.GetKeyDown(KeyCode.RightArrow) || 
			Input.GetKeyDown(KeyCode.Plus) || 
			Input.GetKeyDown(KeyCode.KeypadPlus) || 
			Input.GetButtonDown("Fire1")) {			
			// Cycle to next skybox material in material array
			if (_skybox++ >= materials.Length-1) _skybox = 0;
			// Use the method to change to the new skybox material
			SetSkybox(_skybox);
		}
		
	}
	
	void SetSkybox(int _index) {
		// Get the skybox component of the main camera
		Camera.main.GetComponent<Skybox>().material = materials[_index];
		// Update the GUI texture
		_guiText.text = "Skybox (Left / Right Arrow): " + (_index+1);
	}
}
