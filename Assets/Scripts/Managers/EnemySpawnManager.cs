using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyType //AddNewEnemy: Add type
{
	Level1,
	Level2,
	Level3,
	Boss1,
	IceZombie
}

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
	private GameObject enemyParent;
	private GameObject player;

	public GameObject[] enemyCountArray;
	private int totalEnemyCount = 0;

	public int totalEnemiesToSpawn;

	public int currentWave = 0;
	public int spawnBufferDistance = 7;

	private Vector3 lastBossSpawnPos;
	private float mapSize = GameManager.mapSize;
	int boss1ToSpawn = 0;

	
	public Dictionary<EnemyType, int> enemiesToSpawn = new() //AddNewEnemy: Add type and default value at wave 1 (i think for the last part? lowk dont know)
	{
		{ EnemyType.Level1, 0 },
		{ EnemyType.Level2, 0 },
		{ EnemyType.Level3, 0 },
		{ EnemyType.Boss1, 0 },
		{ EnemyType.IceZombie, 0 }
	};
	private void Start()
	{
		player = GameManager.Instance.player;
		enemyParent = GameManager.Instance.enemies;

		if (totalEnemyCount == 0 && UIManager.Instance.isGameUnpaused)
		{
			if (GameManager.Instance.didLoadSpawnManager)
			{
				Debug.Log("Spawn enemy on load ran." + " Frame: " + Time.frameCount);
				Debug.Log("Total enemy count: " + totalEnemyCount);
				SpawnEnemiesOnLoad();
			}
		}
	}
	void Update()
	{
		enemyCountArray = GameObject.FindGameObjectsWithTag("Enemy");
		totalEnemyCount = enemyCountArray.Length;

		if (totalEnemyCount == 0 && UIManager.Instance.isGameUnpaused && !GameManager.Instance.didLoadSpawnManager)
		{
			currentWave++;

			NumberOfEnemiesToSpawn();

			SpawnEnemyWave();

			GameManager.Instance.wave = currentWave;
		}
	}
	void NumberOfEnemiesToSpawn()
	{
		enemiesToSpawn[EnemyType.Boss1] = 0;

		if (currentWave % 10 == 0)
		{
			boss1ToSpawn++;
			enemiesToSpawn[EnemyType.Boss1] = boss1ToSpawn;
		}

		switch (GameManager.Instance.difficulty)
		{
			case 1:
				if (currentWave > 20)
				{
					enemiesToSpawn[EnemyType.Level1] += 2;
					enemiesToSpawn[EnemyType.Level2] += 2;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 1;
					break;
				} 
				else if (currentWave > 10)
				{
					enemiesToSpawn[EnemyType.Level1] += 2;
					enemiesToSpawn[EnemyType.Level2] += 1;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 1;
					break;
				}
				else if (currentWave > 0)
				{
					enemiesToSpawn[EnemyType.Level1] += 1;
					enemiesToSpawn[EnemyType.Level2] += 1;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 1;
					break;
				}
				break;
			case 2:
				if (currentWave > 20)
				{
					enemiesToSpawn[EnemyType.Level1] += 3;
					enemiesToSpawn[EnemyType.Level2] += 2;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 2;
					break;
				} 
				else if (currentWave > 10)
				{
					enemiesToSpawn[EnemyType.Level1] += 2;
					enemiesToSpawn[EnemyType.Level2] += 1;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 2;
					break;
				}
				else if (currentWave > 0)
				{
					enemiesToSpawn[EnemyType.Level1] += 1;
					enemiesToSpawn[EnemyType.Level2] += 1;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 1;
					break;
				}
				break;
			case 3:
				if (currentWave > 20)
				{
					enemiesToSpawn[EnemyType.Level1] += 3;
					enemiesToSpawn[EnemyType.Level2] += 3;
					enemiesToSpawn[EnemyType.Level3] += 2;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 3;
					break;
				}
				else if (currentWave > 10)
				{
					enemiesToSpawn[EnemyType.Level1] += 2;
					enemiesToSpawn[EnemyType.Level2] += 2;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 2;
					break;
				}
				else if (currentWave > 0)
				{
					enemiesToSpawn[EnemyType.Level1] += 2;
					enemiesToSpawn[EnemyType.Level2] += 1;
					enemiesToSpawn[EnemyType.Level3] += 1;
					// boss1
					enemiesToSpawn[EnemyType.IceZombie] += 2;
					break;
				}
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

		if(NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 4, NavMesh.AllAreas)) 
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
		List<EnemyType> enemyTypes = GameManager.Instance.savedEnemiesTypes;
		List<Vector3> enemyPositions = GameManager.Instance.savedEnemiesPositions;
		List<int> enemyHealths = GameManager.Instance.savedEnemiesHealths;
		if(enemyTypes.Count > 0)
		{
			for (var i = 0; i < enemyTypes.Count; i++)
			{
				int enemyIndex = new();
				switch (enemyTypes[i])
				{
					case EnemyType.Level1:
						enemyIndex = 0;
						break;
					case EnemyType.Level2:
						enemyIndex = 1;
						break;
					case EnemyType.Level3:
						enemyIndex = 2;
						break;
					case EnemyType.Boss1:
						enemyIndex = 3;
						break;
					case EnemyType.IceZombie:
						enemyIndex = 4;
						break;
				}
				GameObject enemy = Instantiate(EnemyDataManager.Instance.enemies[enemyIndex], enemyPositions[i], Quaternion.Euler(90, 0, 0));
				enemy.transform.parent = enemyParent.transform;
				enemy.name = EnemyDataManager.Instance.enemies[enemyIndex].name;
				StartCoroutine(DelayedSetEnemyHealth(enemy, enemyHealths, i));
			}
		}
		StartCoroutine(EnemyDataManager.Instance.AssignEnemiesToLists());
		GameManager.Instance.didLoadSpawnManager = false;
	}
	IEnumerator DelayedSetEnemyHealth(GameObject enemy, List<int> enemyHealths, int i)
	{
		yield return null;
		HealthSystem healthSystem = enemy.GetComponent<HealthSystem>();
		healthSystem.health = enemyHealths[i];
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
	public EnemyData GetEnemyData()
	{
		List<Vector3> enemyPositions = new();
		List<EnemyType> enemyTypes = new();
		List<int> enemyHealths = new();
		foreach (var enemy in enemyCountArray)
		{
			enemyPositions.Add(enemy.transform.position);

			switch (enemy.name)
			{
				case "Enemy 1":
					enemyTypes.Add(EnemyType.Level1);
					enemyHealths.Add(enemy.GetComponent<HealthSystem>().health);
					break;
				case "Enemy 2":
					enemyTypes.Add(EnemyType.Level2);
					enemyHealths.Add(enemy.GetComponent<HealthSystem>().health);
					break;
				case "Enemy 3":
					enemyTypes.Add(EnemyType.Level3);
					enemyHealths.Add(enemy.GetComponent<HealthSystem>().health);
					break;
				case "Boss 1":
					enemyTypes.Add(EnemyType.Boss1);
					enemyHealths.Add(enemy.GetComponent<HealthSystem>().health);
					break;
				case "Ice Zombie":
					enemyTypes.Add(EnemyType.IceZombie);
					enemyHealths.Add(enemy.GetComponent<HealthSystem>().health);
					break;
				default:
					Debug.LogWarning("Unrecognized enemy name: " + enemy.name);
					break;
			}
		}
		return new EnemyData { Positions = enemyPositions, Types = enemyTypes, Healths = enemyHealths };
	}
}
public class EnemyData
{
	public List<Vector3> Positions { get; set; }
	public List<EnemyType> Types { get; set; }
	public List<int> Healths { get; set; }
}
