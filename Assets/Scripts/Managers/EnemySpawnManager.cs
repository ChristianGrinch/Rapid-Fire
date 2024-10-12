using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnManager : MonoBehaviour
{
	public GameObject[] enemy;
	public GameObject enemyParent;
	public GameObject player;

	public GameObject[] enemyCountArray;
	private int enemyCount = 0;

	List<GameObject> level1Enemies = new();
	List<GameObject> level2Enemies = new();
	List<GameObject> level3Enemies = new();
	List<GameObject> boss1Enemies = new();

	public GameObject[] enemyLevel1;
	public GameObject[] enemyLevel2;
	public GameObject[] enemyLevel3;
	public GameObject[] bossLevel1;

	public int currentWave = 0;
	public int spawnBufferDistance = 4;

	private Vector3 lastBossSpawnPos;
	private float mapSize = GameManager.mapSize;

	private Dictionary<EnemyType, int> enemiesToSpawn = new()
	{
		{ EnemyType.Level1, 4 },
		{ EnemyType.Level2, 0 },
		{ EnemyType.Level3, 0 },
		{ EnemyType.Boss1, 0 }
	};
	private enum EnemyType
	{
		Level1,
		Level2,
		Level3,
		Boss1
	}
	void Update()
	{
		enemyCountArray = GameObject.FindGameObjectsWithTag("Enemy");
		enemyCount = enemyCountArray.Length;

		if (enemyCount == 0 && UIManager.Instance.isGameUnpaused)
		{
			if (GameManager.Instance.didLoadSpawnManager)
			{
				SpawnEnemiesOnLoad(GameManager.Instance.enemyLevel1, GameManager.Instance.enemyLevel2, GameManager.Instance.enemyLevel3, GameManager.Instance.bossLevel1);
                GameManager.Instance.didLoadSpawnManager = false;
			}
			else
			{
				currentWave++;

				NumberOfEnemiesToSpawn();

				SpawnEnemyWave();
			}
		}

		StartCoroutine(AssignEnemiesToArray()); // this is INCREDIBLY bad for perfomrance. FIX THIS LATER!!!!
	}

	public IEnumerator AssignEnemiesToArray()
	{
		yield return null;

		level1Enemies = new();
		level2Enemies = new();
		level3Enemies = new();
		boss1Enemies = new();

		foreach (GameObject enemy in enemyCountArray)
		{
			if (enemy.name.Contains("Enemy 1"))
			{
				level1Enemies.Add(enemy);
			}
			else if (enemy.name.Contains("Enemy 2"))
			{
				level2Enemies.Add(enemy);
			}
			else if (enemy.name.Contains("Enemy 3"))
			{
				level3Enemies.Add(enemy);
			}
			else if (enemy.name.Contains("Boss 1"))
			{
				boss1Enemies.Add(enemy);
			}
		}

		enemyLevel1 = level1Enemies.ToArray();
		enemyLevel2 = level2Enemies.ToArray();
		enemyLevel3 = level3Enemies.ToArray();
		bossLevel1 = boss1Enemies.ToArray();
	}

	void NumberOfEnemiesToSpawn()
	{
		if (currentWave % 10 == 0)
		{
			enemiesToSpawn[EnemyType.Boss1] += 1;
		}
		switch (UIManager.Instance.difficulty)
		{
			case 1:
				enemiesToSpawn[EnemyType.Level1] = currentWave + 3;
				enemiesToSpawn[EnemyType.Level2] = currentWave + 1;
				enemiesToSpawn[EnemyType.Level3] = currentWave + -2;
				break;
			case 2:
				enemiesToSpawn[EnemyType.Level1] = currentWave + 5;
				enemiesToSpawn[EnemyType.Level2] = currentWave + 1;
				enemiesToSpawn[EnemyType.Level3] = currentWave + 0;
				break;
			case 3:
				enemiesToSpawn[EnemyType.Level1] = currentWave + 7;
				enemiesToSpawn[EnemyType.Level2] = currentWave + 3;
				enemiesToSpawn[EnemyType.Level3] = currentWave + 2;
				break;
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
			Random.Range(-mapSize, mapSize),
			Random.Range(0, mapSize),
			Random.Range(-mapSize, mapSize)
		);

		NavMeshHit hit;
		if(NavMesh.SamplePosition(randomPosition, out hit, spawnBufferDistance, NavMesh.AllAreas))
		{
			return hit.position;
		}
		return randomPosition;
	}
	void SpawnEnemyWave()
	{
		switch (currentWave % 10)
		{
			case 0:
				{
					for (int i = 0; i < enemiesToSpawn[EnemyType.Boss1]; i++)
					{
						InstantiateEnemy(3);
					}
					for (int i = 0; i < enemiesToSpawn[EnemyType.Level1]; i++)
					{
						InstantiateEnemy(0);
					}

					break;
				}

			default:
				{
					for (int i = 0; i < enemiesToSpawn[EnemyType.Level1]; i++)
					{
						InstantiateEnemy(0);
					}
					for (int i = 0; i < enemiesToSpawn[EnemyType.Level2]; i++)
					{
						InstantiateEnemy(1);
					}
					for (int i = 0; i < enemiesToSpawn[EnemyType.Level3]; i++)
					{
						InstantiateEnemy(2);
					}

					break;
				}
		}

		StartCoroutine(AssignEnemiesToArray());
	}

	public void SpawnEnemiesOnLoad(int enemyLevel1, int enemyLevel2, int enemyLevel3, int bossLevel1)
	{
		for (int i = 0; i < bossLevel1; i++) // must be first so the boss pos can be saved
		{
			InstantiateEnemy(3);
		}
		for (int i = 0; i < enemyLevel1; i++)
		{
			InstantiateEnemy(0);
		}
		for (int i = 0; i < enemyLevel2; i++)
		{
			InstantiateEnemy(1);
		}
		for (int i = 0; i < enemyLevel3; i++)
		{
			InstantiateEnemy(2);
		}
	}

	public void InstantiateEnemy(int type)
	{
		GameObject instantiatedEnemy = Instantiate(enemy[type], GenerateSpawnPosition(type), Quaternion.Euler(90, 0, 0));

		instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent
		instantiatedEnemy.name = enemy[type].name; // Removes (Clone) from name
	}

	// Singleton code -----
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

	// End singleton code -----
}
