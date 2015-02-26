using UnityEngine;
using System.Collections;

public class DropSurface : MonoBehaviour {
	
	//We will need the Drag Items Container
	public GameObject dragItemsContainer;
	//Called when an object is dropped on DropSurface
	public void OnDrop(GameObject dropped)
	{
		//Get the DragItem from the dropped object
		DragItem dragItem = dropped.GetComponent<DragItem>();
		//If it has none, don't go further
		if(dragItem == null) return;
		//Call the RecreateDragItem() method
		RecreateDragItem();
		//Instantiate the defined CreateOnDrop Object
		GameObject newPower = NGUITools.AddChild(this.gameObject, dragItem.CreateOnDrop as GameObject);
		//Set the new selected power in the GameManager
		GameManager.SetPower(newPower.GetComponent<Power>().type);
		//Destroy the dropped Object
		Destroy(dropped);
	}
	
	void RecreateDragItem()
	{
		//If there's already a Power selected
		if(GameManager.SelectedPower != Power.Type.None)
		{
			//Get the selected power's Power.cs script
			Power selectedPowerScript = transform.GetChild(0).GetComponent<Power>();
			//Add the Drag Item to the grid
			NGUITools.AddChild(dragItemsContainer, selectedPowerScript.createOnDestroy as GameObject);
			//Destroy the currently selected Power
			Destroy(selectedPowerScript.gameObject);
		}
	}
		
	void OnPress(bool state)
	{
		if(state)
		{
			//Recreate the DragItem now
			RecreateDragItem();
			//Reset SelectedPower to None
			GameManager.SetPower(Power.Type.None);
			//Force reposition of the grid
			dragItemsContainer.GetComponent<UIGrid>().Reposition();
		}
	}
}
