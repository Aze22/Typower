using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public enum Type
	{
		Fighter,
		Transport,
		Cruiser,
		Bomb,
		Civilian,
		Phaser
	}

	[HideInInspector]
	public int score = 0;

	public Type type;
	//Boolean to check if enemy is hacked or not
	[HideInInspector]
	public bool hacked = false;
	//We will need the Self-Destruct Code Label
	private UILabel codeLabel;
	//We will also need the hacking slider
	private UISlider hackSlider;
	//We will need to store the destructCode
	[HideInInspector]
	public string destructCode = "";
	//We will need a hackSpeed float
	[HideInInspector]
	float hackSpeed = 0.2f;
	public int hp = 1;
	public float speedMultiplier;
	public float hackMultiplier;
	[HideInInspector]
	public bool hackable = true;
	[HideInInspector]
	public bool byPlayer = true;

	private UISprite sprite;

	[HideInInspector]
	public Vector2 size = Vector2.zero;

	[HideInInspector]
	public UIButton button;

	private UITweener movementTween;
	private bool giveReward = true;
	
	public void Initialize(Type enemyType, float _movementDuration)
	{
		sprite = transform.FindChild("Sprite").GetComponent<UISprite>();
		transform.localScale = new Vector3(0,0,1);
		type = enemyType;

		SetParameters();
		//Set its position to a random X and Y of -(enemyHeight/2)
		transform.localPosition = 
			new Vector3(Random.Range(size.x * 0.5f, 3840 - (size.x * 0.5f)), -40, 0);

		//Tween its position towards end of background
		movementTween = TweenPosition.Begin(gameObject, _movementDuration / speedMultiplier,
			new Vector3(transform.localPosition.x, (-2160 + size.y + 53), 0));
		movementTween.ignoreTimeScale = false;
		movementTween.delay = 1;

		//Get the hacking slider
		hackSlider = transform.FindChild("DestructCode").GetComponent<UISlider>();

		//Get the hacking status label
		codeLabel = hackSlider.transform.FindChild("Label").GetComponent<UILabel>();

		//Set the Viewport as target for UIForwardEvents
		GetComponent<UIForwardEvents>().target = transform.parent.parent.gameObject;
	}
	
	void SetParameters()
	{
		switch(type)
		{
		case Type.Fighter:
			hp = 1;
			score = 1200;
			break;
		case Type.Transport:
			hp = 2;
			score = 3300;
			break;
		default:
			break;
		}

		size.x = transform.Find("Sprite").GetComponent<UISprite>().localSize.x;
		size.y = transform.Find("Sprite").GetComponent<UISprite>().localSize.y;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		//Is the collided object a DamageZone?
		if(other.CompareTag("DamageZone"))
		{
			//In that case, hurt the player
			HealthController.Instance.Damage(12f);
			giveReward = false;
			byPlayer = false;
			StartCoroutine(Kill());
			return;
		}
		//Store the collided object's ActiveBarrierController
		ActiveBarrierController barrierController = other.GetComponent<ActiveBarrierController>();

		//If it has none, it's not a Barrier - don't go further
		if(barrierController == null) return;
		//Else, call the ActiveBarrier's method for collision handling 
		barrierController.HitByEnemy(this);
	}

	public void Hit(int damage)
	{
		hp -= damage;

		if(hp <= 0)
			StartCoroutine(Kill());
		else
		{
			//Hit Animation
			StartCoroutine(HitAnimation());
		}

	}

	private IEnumerator HitAnimation()
	{
		float initialDuration = movementTween.duration;
		movementTween.duration = 10000;
		TweenScale.Begin(gameObject, 0.5f, new Vector3(1, 0.7f, 1f));
 		yield return new WaitForSeconds(0.5f);
		TweenScale.Begin(gameObject, 0.5f, new Vector3(1, 1, 1));
		yield return new WaitForSeconds(0.8f);
		movementTween.duration = initialDuration *0.5f;
	}
	
	public IEnumerator Kill()
	{
		//Tween for smooth disappearance
		TweenScale.Begin(gameObject, 0.2f, Vector3.zero);
		//Deactivate the collider2D now
		GetComponent<Collider2D>().enabled = false;
		//Wait end of tween, then destroy the enemy
		yield return new WaitForSeconds(0.2f);
		//Remove enemy from the List
		EnemySpawnController.instance.EnemyDestroyed(this);

		if(byPlayer)
		{
			if (giveReward) {
				ViewportHolder.instance.RewardCheck (transform.position);
				ScoreController.instance.ScoreSpawn (transform.position, score);
			}
			ScoreController.instance.CheckCombos();
		}
		Destroy(gameObject);
	}

	public void SetDestructCode(string randomWordKey)
	{
		//If the randomWordKey is not empty...
		if(hackable && randomWordKey != "")
		{
			//... Get the corresponding localized code
			destructCode = Localization.Get(randomWordKey);
			//Set the Label to "Code Encrypted" 
			codeLabel.text = Localization.Get("CodeEncrypted");
			//Activate button
			button.enabled = true;
		}
		//If the randomWordKey is empty, disable hacking slider
		else
		{
			hackSlider.gameObject.SetActive(false);
			sprite.GetComponent<Collider2D>().enabled = false;
		}
	}
	
	void OnClick()
	{
		//If the enemy has a destruct code, launch hacking
		if(destructCode != "")
			StartCoroutine(Hack());
		else
		{
			ErrorManager.instance.Show(ErrorManager.Type.NotHackable, 3);
		}
	}
	
	IEnumerator Hack()
	{
		//Set the Label to "Hacking..."
		codeLabel.text = Localization.Get("Hacking");
		//While hacking slider is not full
		while(hackSlider.value < 1)
		{
			int barrierCount = ViewportHolder.instance.barrierCount;
			//Increase slider value, framerate independant
			hackSpeed = barrierCount <= 0 ? 0.1f : 0.08f + (ViewportHolder.instance.barrierCount * 0.08f * hackMultiplier);
			hackSlider.value += Time.deltaTime * hackSpeed;
			//Wait for next frame
			yield return null;
		}
		//Make sure slider is at 1
		hackSlider.value = 1;
		//Set the hacked bool to true for this enemy
		hacked = true;
		//Display the Self-Destruct code now
		codeLabel.text = "[99FF99]" + destructCode;
	}
}
