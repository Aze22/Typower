using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EnemySpawnController))]
[CanEditMultipleObjects]

public class EnemySpawnControllerEditor : Editor {

	public override void OnInspectorGUI() {
		
		this.DrawDefaultInspector();
		
		Component component = this.target as Component;
		EnemySpawnController script = component.GetComponent<EnemySpawnController>();
		
		if(script != null){
			foreach(EnemySpawnController.Chances currentChance in script.spawnChances)
			{
				currentChance.name = currentChance.type.ToString();
			}
		}
	}
}
