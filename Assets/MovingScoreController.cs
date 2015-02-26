using UnityEngine;
using System.Collections;

public class MovingScoreController : MonoBehaviour {

	public int value = 0;

	public void ScoreArrived()
	{
		ScoreController.instance.score += value;
		Destroy(gameObject);
	}
}
