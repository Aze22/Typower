using UnityEngine;
using System.Collections;

public class AccelerateCooldown : MonoBehaviour {

	UISlider slider;
	BarrierObjectController barrierCtrlr;

	void OnEnable()
	{
		slider = transform.parent.GetComponent<UISlider>();
		barrierCtrlr = slider.transform.parent.GetComponent<BarrierObjectController>();
	}
	void OnPress(bool state)
	{
		if(state)
		{
			slider.value += (0.33f / barrierCtrlr.initialCooldown);
			barrierCtrlr.currentCoolDown -= (0.33f);
			barrierCtrlr.UpdateCooldownLabel();
		}
	}
}
