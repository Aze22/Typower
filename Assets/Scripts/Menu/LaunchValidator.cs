using UnityEngine;
using System.Collections;

public class LaunchValidator : MonoBehaviour {

	public UIInput nicknameInput;
	public GameObject menuContainer;
	
	void OnClick() 
	{
		//If the nickname input is empty...
		if(string.IsNullOrEmpty(nicknameInput.value))
		{
			//...Show a Nickname error notification for 2.5 sec
			NotificationManager.instance.Show(NotificationManager.Type.Nickname, 2.5f);
		}
		//If there's a nickname but no Power is selected...
		else if(GameManager.SelectedPower == Power.Type.None)
		{
			//...Show a Power error notification for 2.5 sec...
			NotificationManager.instance.Show(NotificationManager.Type.Power, 2.5f);
		}
		//If there is a nickname AND a Power selected...
		else
		{
			//Save the Nickname to PlayerPrefs before launch
			PlayerPrefs.SetString("Nickname", nicknameInput.value);
			//... Load Game Scene
			menuContainer.SendMessage("CloseMenu");
			Invoke("LaunchNow", 0.5f);
		}
	}
	
	void LaunchNow()
	{
		Application.LoadLevel("Game");
	}
}
