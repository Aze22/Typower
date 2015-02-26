using UnityEngine;
using System.Collections;

public class BarrierObjectController : MonoBehaviour {
	
	//We will need the Button and the Label
	private UIButton button;
	private UILabel label;

	private Vector2 autoScrollThreshold;
	private Camera gameCam;

	private UISlider slider;

	public float currentCoolDown = 0;
	public float initialCooldown = 0;

	private bool cooldownFinished = false;

	public bool dragging = false;

	void Awake()
	{
		//Get necessary components at Awake
		button = GetComponentInChildren<UIButton>();
		label = GetComponentInChildren<UILabel>();
		label.text = Localization.instance.Get("Barrier");
		slider = GetComponentInChildren<UISlider>();
		autoScrollThreshold.x = 0.92f;
		autoScrollThreshold.y = 0.90f;
		gameCam = GameObject.Find ("UI Root (2D)/Camera").GetComponent<Camera> ();
	}

	void OnEnable()
	{
		slider.gameObject.SetActive(false);
	}

	//Handle autoscroll
	void OnDrag(Vector2 drag)
	{
		Vector2 autoScrollFinalVal = Vector2.zero;
		Vector2 currentVpPos = gameCam.WorldToViewportPoint (transform.position);

		//Horizontal autoscroll
		if (currentVpPos.x > autoScrollThreshold.x)
			autoScrollFinalVal.x = 0.6f;
		else if (currentVpPos.x < (1 - autoScrollThreshold.x))
			autoScrollFinalVal.x = -0.6f;
	

		//Vertical autoscroll
		if (currentVpPos.y > autoScrollThreshold.y)
			autoScrollFinalVal.y = 0.65f;
		else if (currentVpPos.y - 0.12f < (1 - (autoScrollThreshold.y)))
			autoScrollFinalVal.y = -0.65f;

		KeyboardScroll.instance.autoScroll = autoScrollFinalVal;
	}

	
	public IEnumerator Cooldown(float cooldown)
	{
		//Deactivate the Barrier button and update Color to Disable
		button.isEnabled = false;
		button.UpdateColor(false, true);
		slider.gameObject.SetActive(true);
		StartCoroutine(FillSlider(cooldown));
		currentCoolDown = cooldown;
		initialCooldown = cooldown;

		while(currentCoolDown > 0)
		{
			//Update Label with localized text each second
			label.text = Localization.instance.Get("Wait") + " " + Mathf.CeilToInt(currentCoolDown).ToString() + "s";
			currentCoolDown -= 1;
			//Wait for a second, then return to start of While
			yield return new WaitForSeconds(1);
		}

		if(this != null && gameObject != null)
		{
			collider2D.enabled = false;
			yield return null;
		}

		//If cooldown <= 0
		CooldownFinished();
		yield return new WaitForSeconds(0.20f);
		if(this != null && gameObject != null)
		{
			collider2D.enabled = true;
			yield return null;
		}
	}

	public IEnumerator FillSlider(float cooldown)
	{
		while(slider.value < 1)
		{
			slider.value += (Time.deltaTime / cooldown);
			//Wait for a second, then return to start of While
			yield return null;
		}
		slider.gameObject.SetActive(false);
	}
	
	void CooldownFinished()
	{
		if(this != null && gameObject != null && !cooldownFinished)
		{
			cooldownFinished = true;
			currentCoolDown = 0;
			//Reset the Label's Text to "normal" Barrier
			label.text = Localization.instance.Get("Barrier");
			//Reactivate the Barrier button and update Color to Normal
			button.isEnabled = true;
			button.UpdateColor(true, true);

			//Debug.Log("FUCK");
			//Set its scale to {0,0,0}
			transform.localScale = Vector3.zero;
			//Tween it back to make it appear smoothly
			TweenScale.Begin(gameObject, 0.25f, new Vector3(1,1,1));

			//Show Notification to inform the player
			NotificationManager.instance.Show(NotificationManager.Type.BarrierAvailable, 1.5f);
		}
	}

	public void Kill()
	{
		StopAllCoroutines();
		enabled = false;
		Destroy (this.gameObject);
		Destroy(this);
	}
	
	void OnPress(bool pressed)
	{
		//Invert the Collider2D's state
		collider2D.enabled = !pressed;
		
		//If it has just been dropped
		if(!pressed)
		{
			dragging = false;
			KeyboardScroll.instance.autoScroll = Vector2.zero;
			//Get the target's collider2D
			Collider2D col = UICamera.lastHit.collider;
			//If the target has no collider2D or is not the viewport
			if(col == null || (col.GetComponent<ViewportHolder>() == null && col.GetComponent<Enemy>() == null && col.GetComponent<ActiveBarrierController>() == null && col.GetComponent<KeyController>() == null))
			{
				//Reset its localPosition to {0,0,0}
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
			}
		}
		else
		{
			dragging = true;
			transform.localScale = Vector3.one *0.5f;
		}
	}

	public void UpdateCooldownLabel()
	{
		label.text = Localization.instance.Get("Wait") + " " + Mathf.CeilToInt(currentCoolDown).ToString() + "s";
		if(currentCoolDown <= 0)
		{
			slider.gameObject.SetActive(false);
			StopAllCoroutines();
			CooldownFinished();
		}
	}
}
