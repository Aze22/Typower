using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
		
	public enum Difficulties 
	{
		Normal,
		Hard
	}
	
	//This static variable will contain the selected power
	public static Power.Type SelectedPower = Power.Type.None;
	
	//This static method changes the SelectedPower value
	public static void SetPower(Power.Type newPower)
	{
		SelectedPower = newPower;
	}
	
	public static Difficulties Difficulty = Difficulties.Normal;
	
	public void OnDifficultyChange() 
	{
		//If Difficulty changes to Normal, set Difficulties.Normal
		if(UIPopupList.current.value == "Normal")
			Difficulty = Difficulties.Normal;
		//Otherwise, set to Hard
		else Difficulty = Difficulties.Hard;
	}
	
	public void ExitPressed() 
	{
		//Call the exit function in 0.5s
		Invoke("QuitNow", 0.5f);
	}
	
	void QuitNow()
	{
		Application.Quit();
	}
}