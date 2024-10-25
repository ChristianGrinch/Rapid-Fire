using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }
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
	public GameObject enemyParent;
	public GameObject player;

	public GameObject[] enemyCountArray;
	private int totalEnemyCount = 0;

	public int totalEnemiesToSpawn;

	public int currentWave = 0;
	public int spawnBufferDistance = 7;

	private Vector3 lastBossSpawnPos;
	private float mapSize = GameManager.mapSize;

	private bool runningAssignEnemiesToLists;

	private enum EnemyType //AddNewEnemy: Add type
	{
		Level1,
		Level2,
		Level3,
		Boss1,
		IceZombie
	}
	private Dictionary<EnemyType, int> enemiesToSpawn = new() //AddNewEnemy: Add type and default value at wave 1 (i think for the last part? lowk dont know)
	{
		{ EnemyType.Level1, 4 },
		{ EnemyType.Level2, 0 },
		{ EnemyType.Level3, 0 },
		{ EnemyType.Boss1, 0 },
		{ EnemyType.IceZombie, 0 }
	};
	void Update()
	{
		enemyCountArray = GameObject.FindGameObjectsWithTag("Enemy");
		totalEnemyCount = enemyCountArray.Length;

		if (totalEnemyCount == 0 && UIManager.Instance.isGameUnpaused)
		{
			if (GameManager.Instance.didLoadSpawnManager)
			{
				SpawnEnemiesOnLoad();
				GameManager.Instance.didLoadSpawnManager = false;
			}
			else
			{
				currentWave++;

				NumberOfEnemiesToSpawn();

				SpawnEnemyWave();
			}
		}
	}
	void NumberOfEnemiesToSpawn()
	{
		if (currentWave % 10 == 0)
		{
			enemiesToSpawn[EnemyType.Boss1] += 1;
		}
		switch (GameManager.Instance.difficulty) // KEEP ALL THE STUFF COMMENTED CAUSE THIS MIGHT HAVE BROKEN EVERYTHING LOWK
		{
			case 1:
				int number = 3; 
				// (in reference to int above)
				// Spawns 4 lvl1 enemies on wave 1,
				// Spawns 1 lvl2 enemy on wave 1,
				// Spawns -1 lvl3 enemies on wave 1, (spawns 1 on wave 3),
				// And so on
				foreach (var enemyType in enemiesToSpawn.Keys.ToList())
				{
					if (enemyType == EnemyType.Boss1)
					{
						continue; // Skip to the next iteration
					}

					enemiesToSpawn[enemyType] = currentWave + number;
					number -= 2;
				}
				
				//enemiesToSpawn[EnemyType.Level1] = currentWave + 3;
				//enemiesToSpawn[EnemyType.Level2] = currentWave + 1;
				//enemiesToSpawn[EnemyType.Level3] = currentWave + -2; // Spawns 1 lvl3 on wave 3, then incriments
				//enemiesToSpawn[EnemyType.IceZombie] = currentWave + -3; // Spawns 1 ice zombie on wave 4, then incriments
				break;
			case 2:
				number = 5;
				foreach (var enemyType in enemiesToSpawn.Keys.ToList())
				{
					if (enemyType == EnemyType.Boss1)
					{
						continue; // Skip to the next iteration
					}

					enemiesToSpawn[enemyType] = currentWave + number;
					number -= 2;
				}

				//enemiesToSpawn[EnemyType.Level1] = currentWave + 5;
				//enemiesToSpawn[EnemyType.Level2] = currentWave + 1;
				//enemiesToSpawn[EnemyType.Level3] = currentWave + 0;
				//enemiesToSpawn[EnemyType.IceZombie] = currentWave + -2;
				break;
			case 3:
				number = 7;
				foreach (var enemyType in enemiesToSpawn.Keys.ToList())
				{
					if (enemyType == EnemyType.Boss1)
					{
						continue; // Skip to the next iteration
					}

					enemiesToSpawn[enemyType] = currentWave + number;
					number -= 2;
				}
				//enemiesToSpawn[EnemyType.Level1] = currentWave + 7;
				//enemiesToSpawn[EnemyType.Level2] = currentWave + 3;
				//enemiesToSpawn[EnemyType.Level3] = currentWave + 2;
				//enemiesToSpawn[EnemyType.IceZombie] = currentWave + 1;
				break;
		}

		for(var i = 0; i < EnemyDataManager.Instance.enemies.Length; i++)
		{
			foreach(EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
			{
				totalEnemiesToSpawn += enemiesToSpawn[enemyType];
			}
		}
	}
	private Vector3 GenerateSpawnPosition(int type)
	{
		Vector3 randomSpawnPos = GetRandomNavMeshPosition();

		float posY = type == 3 ? 2 : 0.5f;
		randomSpawnPos.y = posY;

		if (type == 3)
		{
			lastBossSpawnPos = randomSpawnPos;
			return lastBossSpawnPos;
		}

		while (Vector3.Distance(player.transform.position, randomSpawnPos) < spawnBufferDistance 
			|| (Vector3.Distance(lastBossSpawnPos, randomSpawnPos) < 5))
		{
			randomSpawnPos = GetRandomNavMeshPosition();
			randomSpawnPos.y = posY;
		}

		return randomSpawnPos;
	}

	Vector3 GetRandomNavMeshPosition()
	{
		Vector3 randomPosition = new(
			UnityEngine.Random.Range(-mapSize, mapSize),
			UnityEngine.Random.Range(0, mapSize),
			UnityEngine.Random.Range(-mapSize, mapSize)
		);

		NavMeshHit hit;
		if(NavMesh.SamplePosition(randomPosition, out hit, 4, NavMesh.AllAreas)) 
			// the 4 was orginally the spawnBufferDistance, but modifying this number so that the enemies would spawn further away from the player,
			// actually breaks it. so i hardcoded this instead and modified it for the use case above (while loop)
		{
			return hit.position;
		}
		return randomPosition;
	}
	void SpawnEnemyWave()
	{
		foreach(var enemy in enemiesToSpawn)
		{
			for(var j = 0; j < enemy.Value; j++)
			{
				InstantiateEnemy(GetEnemyIndex(enemy.Key));
			}
		}
			
		StartCoroutine(EnemyDataManager.Instance.AssignEnemiesToLists());
	}

	public void SpawnEnemiesOnLoad()
	{
		for(var i = 0; i < GameManager.Instance.enemyCount.Count; i++)
		{
			for(var j = 0; j < GameManager.Instance.enemyCount[i]; j++)
			{
				InstantiateEnemy(i);
			}
		}
		StartCoroutine(EnemyDataManager.Instance.AssignEnemiesToLists());
	}

	public void InstantiateEnemy(int type)
	{
		GameObject instantiatedEnemy = Instantiate(EnemyDataManager.Instance.enemies[type], GenerateSpawnPosition(type), Quaternion.Euler(90, 0, 0));

		instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent
		instantiatedEnemy.name = EnemyDataManager.Instance.enemies[type].name; // Removes (Clone) from name
	}
	public void InstantiateEnemyDebug()
	{
		GameObject instantiatedEnemy = Instantiate(EnemyDataManager.Instance.enemies[0], GenerateSpawnPosition(0), Quaternion.Euler(90, 0, 0));
		instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent
		instantiatedEnemy.name = EnemyDataManager.Instance.enemies[0].name; // Removes (Clone) from name
	}
	int GetEnemyIndex(EnemyType enemyType) //AddNewEnemy: Add another case 
	{
		switch (enemyType)
		{
			case EnemyType.Level1: return 0;
			case EnemyType.Level2: return 1;
			case EnemyType.Level3: return 2;
			case EnemyType.Boss1: return 3;
			case EnemyType.IceZombie: return 4;
			default: return -1; // Handle the case if there's an unknown enemy type
		}
	}
}
