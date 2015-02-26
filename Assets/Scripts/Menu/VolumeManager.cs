using UnityEngine;
using System.Collections;

public class VolumeManager : MonoBehaviour {
	
	//We will need the Slider
	UISlider slider;
	//Declare the Toggle button we'll need
	public UIToggle soundToggle;
	
	void Awake ()
	{
		//Get the Slider
		slider = GetComponent<UISlider>();
		//Set the Slider's value to last saved volume
		slider.value = NGUITools.soundVolume;
		//If volume is at 0, uncheck the Sound Checkbox
		if(NGUITools.soundVolume == 0) soundToggle.value = false;
	}
	
	public void OnVolumeChange ()
	{
		//Change NGUI's UI Sounds volume
		NGUITools.soundVolume = UISlider.current.value;
		//Change the Game AudioListener's volume
		AudioListener.volume = UISlider.current.value;
	}
	
	public void OnSoundToggle()
	{
		float newVolume = 0;
		//If sound toggled ON, set new volume to slider value
		if(UIToggle.current.value)
			newVolume = slider.value;
		//Apply newVolume to volumes
		AudioListener.volume = newVolume;
		NGUITools.soundVolume = newVolume;
	}
}
