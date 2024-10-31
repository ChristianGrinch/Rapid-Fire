using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
	public static EnemyDataManager Instance { get; private set; }
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	//AddNewEnemy: Tag that describes what needs to be modified at a location for new enemy types.
	[Header("Enemy Prefabs")]
	public GameObject[] enemies; // All enemy prefabs - AddNewEnemy: Add enemy prefab in inspector
	[Header("Don't touch in inspector")]
	public string[] enemyNames;
	public GameObject[] enemyCountArray; // All active enemy game objects
	public int totalEnemyCount;

	
	public List<int> enemyCount; // How many enemies there are for each type ([0] is enemy 1, and the value is how many)
	private void Start()
	{
		//GameManager.Instance.enemyCount = new List<int>(new int[enemies.Length]);
		Debug.Log("Gamemanager enemy count: " + GameManager.Instance.enemyCount.Count);
		// Dynamically assign the length of the arrays based on how many enemy types exist
		enemyNames = new string[enemies.Length];
		enemyCount = new List<int>(new int[enemies.Length]);

		for (var i = 0; i < enemies.Length; i++){
			enemyNames[i] = enemies[i].name;
		}
		
	}
	private void Update()
	{
		enemyCountArray = EnemySpawnManager.Instance.enemyCountArray;
		totalEnemyCount = enemyCountArray.Length;
		StartCoroutine(AssignEnemiesToLists()); //TODO: make this not be here, for some reason it like triples the amount of enemies saved if it aint here
	}
	public IEnumerator AssignEnemiesToLists()
	{
		yield return null;

		for (var j = 0; j < enemyCount.Count; j++) // Makes sure the enemy count is 0 before adding to the count later in the method
		{
			enemyCount[j] = 0;
		}

		foreach (GameObject enemy in enemyCountArray)
		{
			if (enemy != null)
			{
				for (var i = 0; i < enemyNames.Length; i++)
				{
					if (enemy.name.Contains(enemyNames[i]))
					{
						enemyCount[i]++;
						break;
					}
				}
			}
		}
	}
}
