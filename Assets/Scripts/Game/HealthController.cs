using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	
	//Static variable that will store this instance
	public static HealthController Instance;
	//We will need the attached slider and a HP value
	UISlider slider;
	public float hp = 100;
	
	void Awake()
	{
		//Store this instance in the Instance variable
		Instance = this;
		//Get the slider Component
		slider = GetComponent<UISlider>();
	}
	
	public void Damage(float dmgValue)
	{
		//Calculate new HP value
		float newHp = hp - dmgValue;
		//Set new HP value with a clamp between 0 and 100
		hp = Mathf.Clamp(newHp, 0, 100);
		//Update the slider to a value between 0 and 1
		slider.value = newHp *0.01f;
		//If hp <= 0, restart level
		if(hp <= 0)
			Application.LoadLevel(Application.loadedLevel);
	}

	public void Heal(float healValue)
	{
		//Calculate new HP value
		float newHp = hp + healValue;
		//Set new HP value with a clamp between 0 and 100
		hp = Mathf.Clamp(newHp, 0, 100);
		//Update the slider to a value between 0 and 1
		slider.value = newHp *0.01f;
	}
}
