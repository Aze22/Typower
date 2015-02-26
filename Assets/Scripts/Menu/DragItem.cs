using UnityEngine;
using System.Collections;

public class DragItem : MonoBehaviour {
	
	public Object CreateOnDrop;
	
	//Method called when the Item is Pressed or Released
	void OnPress(bool pressed)
	{
		//Invert the collider2D's state
		collider2D.enabled = !pressed;
		//If the Item is released
		if(!pressed)
		{
			//Get the last hit collider2D
			Collider2D col = UICamera.lastHit.collider;
			//If there is no collider2D, or no DropSurface behind the Power
			if(col == null || col.GetComponent<DropSurface>() == null)
			{
				//Get the grid in parent objects
				UIGrid grid = NGUITools.FindInParents<UIGrid>(gameObject);
				//If a grid is found, order it to Reposition now
				if(grid != null) grid.Reposition();
			}
		}
	}
}
