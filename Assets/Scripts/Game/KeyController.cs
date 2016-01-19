using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour {

	public string english;
	public string french;
	private UILabel label;

	// Use this for initialization
	void Start () {
		GetComponent<UIForwardEvents>().target = GameObject.Find("UI Root (2D)/Camera/Game/Viewport");
		label = transform.FindChild("Label").GetComponent<UILabel>();

		if(french != "Delete" && english != "Delete")
			label.text = english.ToUpper();

		if(!string.IsNullOrEmpty(french) && french != "Delete" && english != "Delete")
		{
			if(Localization.language == "French")
			{
				if(french == "-1") gameObject.SetActive(false);
				label.text = french;
			}
			else if(string.IsNullOrEmpty(english))
			{
				gameObject.SetActive(false);
				NGUITools.FindInParents<UIGrid>(gameObject).Reposition();
			}
		}

		/*GetComponent<UIButtonScale>().pressed = new Vector3(3,3,3);
		GetComponent<UIButtonScale>().duration = 0.05f;*/
	}

	void OnPress(bool pressed)
	{
		if(!pressed)
		{
			if(french == "Delete" || english == "Delete")
			{
				EnemySpawnController.instance.Delete();
			}
			else
			{
				EnemySpawnController.instance.virtualKeyboardInput += label.text.ToLower();
				Debug.Log(label.text.ToLower());
			}
		}
	}
}
