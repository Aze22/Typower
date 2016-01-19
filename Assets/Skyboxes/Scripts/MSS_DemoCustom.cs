using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MSS_CustomSkybox))]
public class MSS_DemoCustom : MonoBehaviour {
	
	// Custom Skybox Demo Preview Script
	
	// Use this script to specify a number of custom skybox materials 
	// in the materials array which can be cycled using input.
	
	// This script must be attached to game object which also has
	// the MSS_CustomSkybox.cs script and the custom skybox mesh.
	
	// Array of custom skybox materials to cycle through in the demo	
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
		SetCustomSkybox(_skybox);
		
		// Hide the mouse pointer
		Cursor.visible = false;
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
			SetCustomSkybox(_skybox);
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
			SetCustomSkybox(_skybox);
		}
		
	}
	
	void SetCustomSkybox(int _index) {
		// Change the material of the game object to the new material
		gameObject.GetComponent<Renderer>().material = materials[_index];
		// Update the GUI texture
		_guiText.text = "Skybox (Left / Right Arrow): " + (_index+1);
	}
}
