using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {

	public string currentLanguage = "English";
	private Transform line2;
	private Transform line3;
	private GameObject holder;

	void Start()
	{
		//Localization.instance.currentLanguage = "English";
		line2 = transform.FindChild("Holder/Line2Container/Line2");
		line3 = transform.FindChild("Holder/Line3Container/Line3");
		holder = transform.FindChild("Holder").gameObject;
		currentLanguage = Localization.instance.currentLanguage;
		ChangeLanguage(currentLanguage);
		holder.SetActive(false);
	}

	public void ChangeLanguage(string newLanguage)
	{
		if(newLanguage == "English")
		{
			line2.transform.localPosition = new Vector3(85, 0, 0);
			line3.transform.localPosition = new Vector3(85, 0, 0);
		}
		else 
		{
			line2.transform.localPosition = new Vector3(0, 0, 0);
			line3.transform.localPosition = new Vector3(-85, 0, 0);
		}

		holder.SetActive(false);
		holder.SetActive(true);
	}
}
