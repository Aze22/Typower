using UnityEngine;
using System.Collections;

public class ActiveBarrierController : MonoBehaviour {
	
	//We will need the Slider and the Label’s UILocalize
	private UISlider slider;
	private UILocalize loc;
	
	bool built = false;
	
	void Awake()
	{
		//Get necessary components at Awake()
		slider = GetComponentInChildren<UISlider>();
		loc = GetComponentInChildren<UILocalize>();
	}
	
	void Start()
	{
		//Set the UIForwardEvents' target to the viewport
		GetComponent<UIForwardEvents>().target = transform.parent.gameObject;
	}
	
	public IEnumerator Build(float buildTime)
	{
		while(slider.value < 1) {
			slider.value += (Time.deltaTime / buildTime);
			yield return null;
		}
		//When slider value is > 1
		BuildFinished();
	}
	
	private void BuildFinished()
	{
		//Make sure it's at 1
		slider.value = 1;
		//Set the key to "normal" barrier and update Localization
		loc.key = "Barrier";
		//loc.Localize();
		//Set the build value to true and activate collider2D
		built = true;
		GetComponent<Collider2D>().enabled = true;

		//Inform viewport
		ViewportHolder.instance.BarrierAdded();
	}
	
	public void HitByEnemy(Enemy enemy)
	{
		//If the barrier isn't built, don't go further
		if(!built) return;
		//Else, kill the enemy
		enemy.Hit(1);
		GetComponent<Collider2D>().enabled = false;
		//Kill the barrier too
		StartCoroutine(RemoveBarrier());
	}
	
	IEnumerator RemoveBarrier()
	{
		//Tween for smooth disappearance
		TweenScale.Begin(gameObject, 0.2f, Vector3.zero);
		//Notify the Viewport that a Barrier has been removed
		ViewportHolder.instance.BarrierRemoved();
		//Wait for end of tween, then destroy the barrier
		yield return new WaitForSeconds(0.2f);
		Destroy(gameObject);
	}

	void OnClick()
	{
		GetComponent<Collider2D>().enabled = false;
		//Kill the barrier too
		StartCoroutine(RemoveBarrier());
	}
}
