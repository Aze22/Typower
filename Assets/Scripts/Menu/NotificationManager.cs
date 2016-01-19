using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour {
	
	//Create an Enum to define Notification Type
	public enum Type
	{
		Nickname,
		Power,
		BarrierAvailable,
	}
	//Declare necessary variables
	public UILocalize loc;
	public Type type;
	//Store the Notification to access it in static methods
	public static NotificationManager instance;
	
	void Awake()
	{
		//Set the static instance to this NotificationManager
		instance = this;
		//Deactivate Notification GameObject on awake
		gameObject.SetActive(false);
	}
	
	void OnEnable () {
		//Start a TweenScale of 1.5 second towards {0,0,0}
		TweenScale tween = TweenScale.Begin(this.gameObject, 0.5f, new Vector3(1,1,1));
		//Add an easing in and out method to our Tween
		tween.method = UITweener.Method.EaseInOut;
		//Set the Localize key to Type + “Notification” 
		loc.key = type.ToString() + "Notification";
		//Force Update the UILocalize with new key
		//loc.Localize();
	}
	
	public void Show(Type notificationType, float duration)
	{
		//If there is no current Notification
		if(!gameObject.activeInHierarchy)
		{
			//Set the asked Notification type
			type = notificationType;
			//Enable our Notification on scene
			gameObject.SetActive(true);
			//Start Couroutine to remove in asked duration
			StartCoroutine(Remove(duration));
		}
	}
	
	public IEnumerator Remove(float duration)
	{
		//Wait for the Notification display duration 
		yield return new WaitForSeconds(duration);
		//Start the TweenScale to disappear
		TweenScale.Begin(gameObject, 0.5f, new Vector3(0,0,1));
		//Wait for 0.5s, the duration of the TweenScale
		yield return new WaitForSeconds(0.5f);
		//Deactivate the Notification GameObject
		gameObject.SetActive(false);
	}	
}
