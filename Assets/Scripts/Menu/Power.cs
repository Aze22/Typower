using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour {

	//Declare an enum to define type of Power
	public enum Type 
	{
		None,
		Time,
		Bomb
	}
	//Declare a Type variable to choose it in Inspector
	public Type type;
	//Object variable to define the DragItem to recreate
	public Object createOnDestroy;

}
