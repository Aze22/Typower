using UnityEngine;
using System.Collections;

public class TooltipManager : MonoBehaviour {
	
	//Enum to define which type of tooltip must be shown
	public enum Type
	{
		Bomb,
		Time
	}
	
	//Declare the Type enum variable
	public Type type; 
	
	//Called when a Tooltip event is triggered on this object
	void OnTooltip(bool state)
	{
		//If state is true, create a new Tooltip depending on the type
		if(state)
			UITooltip.ShowText(Localization.Get(type.ToString() + "Tooltip"));
		//If state is false, hide tooltip by setting an empty string
		else
			UITooltip.ShowText("");
	}
}
