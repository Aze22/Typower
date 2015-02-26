using UnityEngine;
using UnityEditor;
using System.Collections;

// Helper script to instantiate a Custom Skybox prefab

public class MSS_CreateCustomSkybox : Editor {
	
	// Add option to the GameObject menu
	[MenuItem("GameObject/Create Other/Custom Skybox")]
	
	static void CreateCustomSkybox() {
		// Path to the skybox custom prefab. Change path if you rename / move the assets
		string pathPrefab = "Assets/Mobile SPACE Skyboxes 01/Prefabs/Skybox Custom.prefab";
		
		// Instantiate the Custom Skybox object
		GameObject _sGameObject  = (GameObject) AssetDatabase.LoadAssetAtPath(pathPrefab, typeof(GameObject));
		PrefabUtility.InstantiatePrefab((GameObject) _sGameObject);	
	}
}
