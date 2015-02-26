//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Very simple script that can be attached to a slider and will control the volume of all sounds played via NGUITools.PlaySound,
/// which includes all of UI's sounds.
/// </summary>

[RequireComponent(typeof(UISlider))]
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
public class UISoundVolume : MonoBehaviour
{
	UISlider mSlider;
	//Declare the Toggle button we'll need
	public UIToggle soundToggle;

	void Awake ()
	{
		mSlider = GetComponent<UISlider>();
		mSlider.value = NGUITools.soundVolume;
		EventDelegate.Add(mSlider.onChange, OnChange);
		//If volume at 0, uncheck the Sound Checkbox
		if( mSlider.value == 0 ) soundToggle.value = false;
	}
		
	void OnChange ()
	{
		NGUITools.soundVolume = UISlider.current.value;
		//Add this line to change game volume too:
		AudioListener.volume = UISlider.current.value;
	}
	
	public void OnSoundToggle()
	{
		float newVolume = 0;
		//If sound toggled on, set new volume to slider value
		if( UIToggle.current.value )
			newVolume = mSlider.value;
		//Apply newVolume to volumes
		AudioListener.volume = newVolume;
		NGUITools.soundVolume = newVolume;
	}
}
