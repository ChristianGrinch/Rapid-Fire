using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
	public GameObject[] enemy;

	public GameObject enemyParent;

	private GameObject[] enemyCountArray;
	private int enemyCount = 0;
	public int currentWave = 0;

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
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		enemyCountArray = GameObject.FindGameObjectsWithTag("Enemy");
		enemyCount = enemyCountArray.Length;

		if (enemyCount == 0)
		{
			currentWave++;

            NumberOfEnemiesToSpawn();

            SpawnEnemyWave();
		}

	}

	void NumberOfEnemiesToSpawn()
	{
		if (currentWave % 2 == 0)
		{
            enemiesToSpawn[EnemyType.Boss1] += 1;
        }
		else
		{
            enemiesToSpawn[EnemyType.Level1] += 1;
            enemiesToSpawn[EnemyType.Level2] += 1;
            enemiesToSpawn[EnemyType.Level3] += 1;
        }
	}
	private Vector3 GenerateSpawnPosition(int type)
	{
        if (type == 3) // if its a boss:
        {
            float randomPosX = Random.Range(-20f, 20f);
            float randomPosZ = Random.Range(-20f, 20f);

            Vector3 randomPos = new(randomPosX, 2.5f, randomPosZ);

            return randomPos;
        }
        else
        {
            float randomPosX = Random.Range(-20f, 20f);
            float randomPosZ = Random.Range(-20f, 20f);

            Vector3 randomPos = new(randomPosX, 1, randomPosZ);

            return randomPos;
        }

    }
	 
	void SpawnEnemyWave()
	{
		for (int i = 0; i < enemiesToSpawn[EnemyType.Level1]; i++)
		{
			InstantiateEnemy(0);
		}
		for(int i = 0; i < enemiesToSpawn[EnemyType.Level2]; i++)
		{
			InstantiateEnemy(1);
		}
		for (int i = 0; i < enemiesToSpawn[EnemyType.Level3]; i++)
		{
			InstantiateEnemy(2);
		}
        for (int i = 0; i < enemiesToSpawn[EnemyType.Boss1]; i++)
        {
            InstantiateEnemy(3);
        }
    }

	void InstantiateEnemy(int type)
	{
		GameObject instantiatedEnemy = Instantiate(enemy[type], GenerateSpawnPosition(type), Quaternion.Euler(90, 0, 0));

		instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent
        instantiatedEnemy.name = enemy[type].name; // Removes (Clone) from name
    }

	public static SpawnEnemy Instance { get; private set; }

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
		DontDestroyOnLoad(gameObject);
	}
}
