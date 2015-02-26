using UnityEngine;
using System.Collections;

// Demo Script to preview skyboxes on mobile devices

// Uses accelerometer (tilt) to rotate

public class MSS_MobileLook : MonoBehaviour {
	
	// Sensitivity multiplier
    public float sensitivity = 1.0f; 
      
    void Update() {
		// Use accelerometer input (mobile device tilting) to rotate camera
		// (NOTE: You may wish to change x and y for Unity 3.x)
		transform.Rotate(transform.right, Input.acceleration.y * sensitivity * Time.deltaTime);
		transform.Rotate(transform.up, Input.acceleration.x * sensitivity * Time.deltaTime);		

	}

}