using UnityEngine;
using System.Collections;

public class UpdatePanel : MonoBehaviour 
{
	public void UpdateNow()
	{
		//Force a drag of {0, 0, 0} to update Panel
		GetComponent<UIScrollView>().MoveRelative(Vector3.zero);
	}
}
