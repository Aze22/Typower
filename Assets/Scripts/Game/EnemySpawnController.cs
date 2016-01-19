using UnityEngine;
using System.Collections;
//Include Lists
using System.Collections.Generic;

public class EnemySpawnController : MonoBehaviour {


	//Random-control variables
	public int firstEnemyDelay = 1;

	public int difficultyStep = 10;
	//Min and Max intervals between 2 spawns
	public float minInterval = 3;
	public float minIntervalMultiplier = 0.9f;
	public float maxInterval = 12;
	public float maxIntervalMultiplier = 0.85f;

	//Min and Max Enemy MovementTime
	public float minMovementTime = 20;
	public float minMovementTimeMultiplier = 0.85f;
	public float maxMovementTIme = 50;
	public float maxMovementTimeMultiplier = 0.75f;



	//Chance for each enemy to have a destructCode
	public float destructCodeChance = 60;


	//Array of strings to store destructCodes keys
	public string[] easyWordKeys;
	public string[] mediumWordKeys;
	public string[] hardWordKeys;
	public string[] veryHardWordKeys;

	//We will need a list of enemies
	List<Enemy> enemies;

	//We will need a static instance of this script
	public static EnemySpawnController instance;

	//This will store the current word typed by the player
	public string currentWord;
	
	[System.Serializable]
	public class Chances
	{
		[HideInInspector]
		public string name = "Select a Type";
		public Enemy.Type type;
		public float spawnStart;
		public float spawnEnd;
		public float hackableChance;
	}
	

	[HideInInspector]
	public float enemiesDestroyed = 0;

	public List<Chances> spawnChances = new List<Chances>();

	[HideInInspector]
	public string virtualKeyboardInput = "";

	public int hackKillBonus = 1000;

	[HideInInspector]
	public UILabel inputLabel;

	[HideInInspector]
	public GameObject deleteButton;
	
	void Awake()
	{

		//Store the instance of this script
		instance = this;
		//Initialize the List
		enemies = new List<Enemy>();
	}
		
	//Coroutine that spawns enemies
	IEnumerator SpawnEnemy(float delay)
	{
		//Loop while the game is running
		while(true){
			//Wait for the correct delay
			yield return new WaitForSeconds(delay);

			Enemy.Type newEnemyType = GetEnemyType();


			//Create a new enemy, stock its Enemy
			Enemy newEnemy =
				NGUITools.AddChild(gameObject, Resources.Load("Enemies/" + newEnemyType.ToString()) as GameObject).GetComponent<Enemy>();

			//Initialize it with random speed
			newEnemy.Initialize(newEnemyType, Random.Range (minMovementTime, maxMovementTIme));
			//Set the new random delay
			delay = Random.Range(minInterval, maxInterval);
			//Create a new empty string for destruct code
			string randomCode = "";

			destructCodeChance = GetChance(newEnemyType).hackableChance;

			//If the random is valid, get a random word
			if(Random.Range(0f,100f) < destructCodeChance)
			{
				newEnemy.hackable = true;
				randomCode = GetRandomWord();
			}
			else newEnemy.hackable = false;
			//Set the enemy's the DestructCode
			newEnemy.SetDestructCode(randomCode);
			//Add the enemy to the list of enemies
			enemies.Add(newEnemy);
		}
	}

	public Chances GetChance(Enemy.Type type)
	{
		foreach(Chances currentChance in spawnChances)
		{
			if(currentChance.type == type)
				return currentChance;
		}
		return null;
	}
	
	Enemy.Type GetEnemyType()
	{
		float randomValue = Random.Range(0f, 100f);
		Enemy.Type selectedType = Enemy.Type.Fighter;
		
		foreach(Chances chance in spawnChances)
		{
			if(randomValue >= chance.spawnStart && randomValue < chance.spawnEnd)
			{
				selectedType = chance.type;
				//Test
			}
		}
		return selectedType;
	}
	
	string GetRandomWord()
	{
		//Return a random Word Key
		string[] wordKeysToUse = GetCurrentDiffWordKeys();

		return wordKeysToUse[Random.Range(0, wordKeysToUse.Length)];
	}

	public string[] GetCurrentDiffWordKeys()
	{
		string[] wordKeysToReturn = easyWordKeys;

		if(enemiesDestroyed > 15 && enemiesDestroyed <= 35)
		{
			wordKeysToReturn = mediumWordKeys;
		}
		else if(enemiesDestroyed > 35 && enemiesDestroyed <= 55)
		{
			wordKeysToReturn = hardWordKeys;
		}
		else if(enemiesDestroyed > 55)
		{
			wordKeysToReturn = veryHardWordKeys;
		}

		return wordKeysToReturn;
	}
	
	void Start ()
	{
		//Start the Spawn Coroutine with first delay
		StartCoroutine(SpawnEnemy(firstEnemyDelay));

		//Initialize Input Label
		inputLabel.text = /*Localization.Get("AwaitingInput")*/"";
	}
	
	void Update()
	{
		string matchedCode = "";

		//If the player has typed a character
		if(Input.inputString != "" || virtualKeyboardInput != "") 
		{
			//Add this new character to the currentWord
			currentWord += (Input.inputString + virtualKeyboardInput);
			virtualKeyboardInput = "";
			//We need to know if the code matches at least 1 enemy
			bool codeMatches = false;
			//Check enemies' destruct codes one by one
			foreach(Enemy enemy in enemies)
			{
				//If the enemy has a destruct code AND is hacked
				if(enemy.destructCode != "" && enemy.hacked)
				{
					//currentWord contain the destruct code?
					if(currentWord.Contains(enemy.destructCode))
					{
						//Yes - Destroy it and update our bool
						if(enemy.GetComponent<Collider2D>().enabled) 
						{
							enemy.score += hackKillBonus;
							StartCoroutine(enemy.Kill());
							codeMatches = true;
							matchedCode = enemy.destructCode;
						}
					}
				}
			}
			//Did the word match at least 1 enemy?
			if(codeMatches)
			{
				inputLabel.text = currentWord.Replace(matchedCode, ("[99FF99]" + matchedCode));
				Invoke("RemoveHighlight", 2f);
				//In that case, reset the currentWord to empty
				currentWord = "";
				ShowDeleteButton(false);
			}
			else
			{
				inputLabel.text = currentWord;
				CancelInvoke("RemoveHighlight");
				ShowDeleteButton(true);
			}
		}
	}

	private void RemoveHighlight()
	{
		inputLabel.text = "";
	}
	
	public void EnemyDestroyed(Enemy destroyedEnemy)
	{
		//Remove the destroyed enemy from the List
		enemies.Remove(destroyedEnemy);
		enemiesDestroyed++;

		if(enemiesDestroyed % difficultyStep == 0)
			IncreaseDifficulty();
	}

	public void IncreaseDifficulty()
	{
		minInterval *= minIntervalMultiplier;
		maxInterval *= maxIntervalMultiplier;
		minMovementTime *= minMovementTimeMultiplier;
		maxMovementTIme *= maxMovementTimeMultiplier;
	}

	public void Delete()
	{
		if(currentWord.Length > 0) 
		{
			currentWord = currentWord.Remove(currentWord.Length-1);
			inputLabel.text = currentWord;
		}

		if(currentWord.Length <= 0)
			ShowDeleteButton(false);
	}

	public void ShowDeleteButton(bool state)
	{
		deleteButton.SetActive(state);
	}
}
