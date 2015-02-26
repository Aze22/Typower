using UnityEngine;
using System.Collections;

public class AppearFromAbove : MonoBehaviour 
{
	void Start () 
	{
		//First, set the Menu's Y position to be out of screen
		this.transform.localPosition = new Vector3(0, 1080, 0);
		//Start a TweenPosition of 1.5 second towards {0,0,0}
		TweenPosition tween = TweenPosition.Begin(this.gameObject, 1.5f, Vector3.zero);
		//Add a delay to our Tween
		tween.delay = 1f;
		//Add an easing in and out method to our Tween
		tween.method = UITweener.Method.EaseInOut;
	}
	
	void CloseMenu()
	{
		//Tween the menu's scale to zero
		TweenScale.Begin(this.gameObject, 0.5f, Vector3.zero);
	}
}