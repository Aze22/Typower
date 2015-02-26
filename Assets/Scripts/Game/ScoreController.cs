using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

	[HideInInspector]
	public UILabel label;
	[HideInInspector]
	private int _score = 0;

	private UILabel comboLabel = null; 
	private float timeSinceLastKill = 0;

	public float comboCooldown = 2f;
	private int currentComboCount = 0;

	public int score {
		get{
			return _score;
		}
		set{
			_score = value;
			if(label != null) label.text = _score.ToString();
			//Debug.Log(_score);
		}
	}

	public static ScoreController instance;

	void Awake()
	{
		instance = this;
		score = 0;
		label = transform.FindChild ("Label").GetComponent<UILabel> ();
		label.text = "0";
		comboLabel = transform.FindChild ("ComboLabel").GetComponent<UILabel> ();
		comboLabel.text = "";
		ShowComboLabel(false);
	}

	public void ScoreSpawn(Vector3 spawnPos, int scoreToAdd)
	{
		GameObject movingScore = NGUITools.AddChild(transform.parent.gameObject, Resources.Load("MovingScore") as GameObject);
		movingScore.transform.position = spawnPos;
		movingScore.transform.FindChild("Label").GetComponent<UILabel>().text = "+ " + scoreToAdd.ToString();
		MovingScoreController movingScoreCtrlr = movingScore.GetComponent<MovingScoreController> ();
		movingScoreCtrlr.value = scoreToAdd;
		UITweener tween = TweenPosition.Begin(movingScore, 1.2f, new Vector3(-85, -25, 0));
		EventDelegate.Add(tween.onFinished, movingScoreCtrlr.ScoreArrived);
	}

	void Update()
	{
		timeSinceLastKill -= Time.deltaTime;
	}

	public void CheckCombos()
	{
		if(timeSinceLastKill <= comboCooldown)
		{
			currentComboCount++;
			timeSinceLastKill = comboCooldown;

			ShowComboLabel(true);
			UpdateComboLabel();
			StopAllCoroutines();	
			StartCoroutine(ComboLabelTimer());
		}
	}

	public void ShowComboLabel(bool state)
	{
		if(!comboLabel.gameObject.activeInHierarchy && state)
		{
			comboLabel.gameObject.SetActive (true);
		}
		else if(comboLabel.gameObject.activeInHierarchy && !state)
		{
			comboLabel.text = "";
			comboLabel.gameObject.SetActive (false);
			currentComboCount = 0;
		}
	}

	public void UpdateComboLabel()
	{ 
		comboLabel.text = "[00FFAA]x" + currentComboCount.ToString() + "[-] Combo!";
	}

	private IEnumerator ComboLabelTimer()
	{
		comboLabel.transform.localScale = Vector3.one *1.5f;
		TweenScale.Begin(comboLabel.gameObject, comboCooldown, Vector3.zero);
		yield return new WaitForSeconds(comboCooldown);
		ShowComboLabel(false);
	}
}
