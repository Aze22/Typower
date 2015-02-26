using UnityEngine;
using System.Collections;

public class RewardController : MonoBehaviour {

	public enum Type {
		Health,
		Bomb,
		Time
	}

	public Type type;
	public float timeOut = 0;

	delegate void ExecuteAction();
	ExecuteAction executeAction;

	public int scoreToAdd = 0;

	void Start()
	{
		transform.localScale = new Vector3(0,0,1);

		if (type == Type.Bomb)
		{
			executeAction = BombAction;
			scoreToAdd = 800;
		}
		else if (type == Type.Time)
		{
			executeAction = TimeAction;
			scoreToAdd = 800;
		}
		else
		{
			executeAction = HealthAction;
			scoreToAdd = 350;
		}

		if(timeOut > 0)
			StartCoroutine(TimeOut());
	}

	IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(timeOut);
		TweenScale.Begin(gameObject, 2, Vector3.zero);
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
	}

	void OnClick() {
		executeAction();
	}

	void HealthAction()
	{
		if(HealthController.Instance.hp < 100) 
		{
			transform.parent = transform.parent.parent.parent.FindChild("UI/Anchor - Top");
			ScoreController.instance.ScoreSpawn(transform.position, scoreToAdd);
			StartCoroutine(Heal());
		}
		else
		{
			ErrorManager.instance.Show(ErrorManager.Type.HealthFull, 2f);
		}
	}

	IEnumerator Heal()
	{
		TweenPosition.Begin(gameObject, 0.5f, Vector3.zero);
		yield return new WaitForSeconds(0.5f);
		HealthController.Instance.Heal(25f);
		Destroy(gameObject);
	}

	void BombAction()
	{
		
	}

	void TimeAction()
	{
		
	}
}

