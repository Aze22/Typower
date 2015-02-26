using UnityEngine;
using System.Collections;

public class ViewportHolder : MonoBehaviour {

	public float rewardChance;
	//We need our two barriers Prefabs
	[HideInInspector]
	public Object barrierObjectPrefab;
	[HideInInspector]
	public Object activeBarrierPrefab;
	
	//We need the BarrierObject container
	[HideInInspector]
	public GameObject barrierContainer;
	
	//Set it to public
	[HideInInspector]
	public int barrierCount = 0;
	
	//Have an Instance
	public static ViewportHolder instance;

	void Awake()
	{
		instance = this;
	}

	void OnDrop(GameObject droppedObj)
	{
		//Get the dropped object's BarrierObjectController
		BarrierObjectController barrierObj = droppedObj.GetComponent<BarrierObjectController>();
		
		//If it actually has one, destroy the droppedObj
		if(barrierObj != null){
			barrierObj.Kill();
			RecreateBarrierObject();
			CreateActiveBarrier(droppedObj.transform);
		}
	}
	
	void RecreateBarrierObject()
	{
		//Add a BarrierObject to the container
		Transform newBarrierTrans = NGUITools.AddChild(barrierContainer, barrierObjectPrefab as GameObject).transform;
		//Reset its localPosition to {0,0,0}
		newBarrierTrans.localPosition = Vector3.zero;
		//Start the new BarrierObject's Cooldown Coroutine
		StartCoroutine(newBarrierTrans.GetComponent<BarrierObjectController>().Cooldown((barrierCount +1) *3));
	}
	
	void CreateActiveBarrier(Transform barrierObjectTrans)
	{
		//Add an ActiveBarrier to the Viewport
		Transform newActiveBarrierTrans = NGUITools.AddChild(gameObject, activeBarrierPrefab as GameObject).transform;
		//Set position to the droppedObject's position
		newActiveBarrierTrans.position = barrierObjectTrans.position;
		//Start the Build Coroutine with the correct buildTime
		StartCoroutine(newActiveBarrierTrans.GetComponent<ActiveBarrierController>().Build(barrierCount <= 0 ? 1 : barrierCount *2));
	}

	public void BarrierRemoved()
	{
		//Decrease the barrierCount value
		barrierCount--;
	}

	public void BarrierAdded()
	{
		//Decrease the barrierCount value
		barrierCount++;
	}

	public void RewardCheck(Vector3 spawnPos)
	{
		if(Random.Range(0f, 100f) < rewardChance)
		{
			RewardController.Type newRewardType = RewardController.Type.Health;
			GameObject reward = NGUITools.AddChild(gameObject, Resources.Load("Rewards/" + newRewardType.ToString()) as GameObject);
			reward.transform.position = spawnPos;
			reward.GetComponent<RewardController>().type = newRewardType;
		}
	}
}
