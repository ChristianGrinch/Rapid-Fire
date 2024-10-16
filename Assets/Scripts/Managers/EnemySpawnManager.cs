using System.Collections;
using System.Collections.Generic;
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
    public EnemySpawnManager enemySpawnManager;
    private void Start()
    {
		enemySpawnManager = this;
    }

    // When making a new enemy, add them in the places that have the ~~~~~~~~~~ENEMY tag

    //~~~~~~~~~~ENEMY (ADD IN INSPECTOR)
    public GameObject[] enemy;
	public GameObject enemyParent;
	public GameObject player;

	public GameObject[] enemyCountArray;
	private int enemyCount = 0;

    //~~~~~~~~~~ENEMY
    public List<GameObject> level1Enemies = new();
    public List<GameObject> level2Enemies = new();
    public List<GameObject> level3Enemies = new();
    public List<GameObject> boss1Enemies = new();
	public List<GameObject> iceZombie = new();

	public int currentWave = 0;
	public int spawnBufferDistance = 4;

	private Vector3 lastBossSpawnPos;
	private float mapSize = GameManager.mapSize;

    //~~~~~~~~~~ENEMY
    private enum EnemyType
    {
        Level1,
        Level2,
        Level3,
        Boss1,
		IceZombie
    }
    //~~~~~~~~~~ENEMY
    private Dictionary<EnemyType, int> enemiesToSpawn = new()
	{
		{ EnemyType.Level1, 4 },
		{ EnemyType.Level2, 0 },
		{ EnemyType.Level3, 0 },
		{ EnemyType.Boss1, 0 },
		{ EnemyType.IceZombie, 0 }
	};
	void Update()
	{
		Debug.Log(SaveManager.Instance.enemyLevel1);
        enemyCountArray = GameObject.FindGameObjectsWithTag("Enemy");
		enemyCount = enemyCountArray.Length;

		if (enemyCount == 0 && GameManager.Instance.isGameUnpaused)
		{
			Debug.Log(currentWave);
			if (SaveManager.Instance.didLoadSpawnManager) // runs at the start to 
			{
				Debug.Log("sus");
				SpawnEnemiesOnLoad();
                SaveManager.Instance.didLoadSpawnManager = false;
			}
			else
			{
				Debug.Log("rgwe");
				currentWave++;

				NumberOfEnemiesToSpawn();

				SpawnEnemyWave();
			}
		}

		StartCoroutine(AssignEnemiesToLists()); // this is INCREDIBLY bad for perfomrance. FIX THIS LATER!!!!
	}

	public IEnumerator AssignEnemiesToLists()
	{
		yield return null;

		level1Enemies = new();
		level2Enemies = new();
		level3Enemies = new();
		boss1Enemies = new();

        //~~~~~~~~~~ENEMY
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
			else if(enemy.name.Contains("Ice Zombie"))
			{
				iceZombie.Add(enemy);
			}
		}
	}

	void NumberOfEnemiesToSpawn()
	{
		if (currentWave % 10 == 0)
		{
			enemiesToSpawn[EnemyType.Boss1] += 1;
		}
        //~~~~~~~~~~ENEMY
        switch (GameManager.Instance.difficulty)
		{
			case 1:
				enemiesToSpawn[EnemyType.Level1] = currentWave + 3;
				enemiesToSpawn[EnemyType.Level2] = currentWave + 1;
				enemiesToSpawn[EnemyType.Level3] = currentWave + -2; // Spawns 1 lvl3 on wave 3, then incriments
				enemiesToSpawn[EnemyType.IceZombie] = currentWave + -3; // Spawns 1 ice zombie on wave 4, then incriments
				break;
			case 2:
				enemiesToSpawn[EnemyType.Level1] = currentWave + 5;
				enemiesToSpawn[EnemyType.Level2] = currentWave + 1;
				enemiesToSpawn[EnemyType.Level3] = currentWave + 0;
				enemiesToSpawn[EnemyType.IceZombie] = currentWave + -2;
				break;
			case 3:
				enemiesToSpawn[EnemyType.Level1] = currentWave + 7;
				enemiesToSpawn[EnemyType.Level2] = currentWave + 3;
				enemiesToSpawn[EnemyType.Level3] = currentWave + 2;
				enemiesToSpawn[EnemyType.IceZombie] = currentWave + 1;
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
        //~~~~~~~~~~ENEMY
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
                    for (int i = 0; i < enemiesToSpawn[EnemyType.IceZombie]; i++)
                    {
                        InstantiateEnemy(4);
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
					for(int i = 0; i < enemiesToSpawn[EnemyType.IceZombie]; i++)
					{
						InstantiateEnemy(4);
					}
					break;
				}
		}

		StartCoroutine(AssignEnemiesToLists());
	}

	public void SpawnEnemiesOnLoad()
	{
        //~~~~~~~~~~ENEMY
        for (int i = 0; i < SaveManager.Instance.bossLevel1; i++) // must be first so the boss pos can be saved
		{
			InstantiateEnemy(3);
		}
		for (int i = 0; i < SaveManager.Instance.enemyLevel1; i++)
		{
			InstantiateEnemy(0);
		}
		for (int i = 0; i < SaveManager.Instance.enemyLevel2; i++)
		{
			InstantiateEnemy(1);
		}
		for (int i = 0; i < SaveManager.Instance.enemyLevel3; i++)
		{
			InstantiateEnemy(2);
		}
        for (int i = 0; i < SaveManager.Instance.iceZombie; i++)
        {
            InstantiateEnemy(4);
        }
    }

	public void InstantiateEnemy(int type)
	{
		GameObject instantiatedEnemy = Instantiate(enemy[type], GenerateSpawnPosition(type), Quaternion.Euler(90, 0, 0));

		instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent
		instantiatedEnemy.name = enemy[type].name; // Removes (Clone) from name
	}

}
