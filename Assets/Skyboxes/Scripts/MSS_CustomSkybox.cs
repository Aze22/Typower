using UnityEngine;
using System.Collections;

// Mobile SPACE Skyboxes - Custom Skybox

// Enables a mesh which is positioned at the parentCamera which
// renders a custom skybox for the camera.

// The skybox is only 1x1x1 Unity units in size but it renders using
// a custom skybox shader which executes in the background render queue
// to ensure that the skybox is rendered before any scene objects.

public class MSS_CustomSkybox : MonoBehaviour {
	
	// Reference to the parent camera. Specify your primary camera or
	// if left unassigned the script will automatically select the main camera.
	public Transform parentCamera;
	
	void OnEnable () {
		// On enable, enable the MeshRenderer which is by default disabled in the prefab for work in the scene view
		MeshRenderer _mr = gameObject.GetComponent<MeshRenderer>();		
		if (_mr != null) 
			_mr.enabled = true;		
		
		// If parent Camera is not specified, select the main camera
		if (parentCamera == null) 
			parentCamera = Camera.main.transform;
		
	}

	void OnDisable () {
		// On disable, hide the MeshRenderer for work in the scene view
		MeshRenderer _mr = gameObject.GetComponent<MeshRenderer>();
		if (_mr != null) 
			_mr.enabled = true;
	}
		
	void LateUpdate() {
		// If a parent camera exists...
		if (parentCamera != null) 
			// Set the position of the skybox to the same location as the camera
			transform.position = parentCamera.position - new Vector3(0,0.5f,0);
		
	}
}
