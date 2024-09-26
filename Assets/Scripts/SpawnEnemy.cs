using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
	public GameObject[] enemy;

	private int enemyType;
	private int randomEnemy;

	public GameObject enemyParent;

	private GameObject[] enemyCountArray;
	private int enemyCount = 0;
	public int currentWave = 1;

	private int[] enemiesToSpawnArray = new int[] {4, 0, 0};

    // Start is called before the first frame update
    void Start()
	{
		SpawnEnemyWave();
	}

	// Update is called once per frame
	void Update()
	{
		enemyCountArray = GameObject.FindGameObjectsWithTag("Enemy");
		enemyCount = enemyCountArray.Length;

		if (enemyCount == 0)
		{
			currentWave++;

            SpawnEnemyWave();

            NumberOfEnemiesToSpawn();

        }

	}

	void NumberOfEnemiesToSpawn()
	{
			enemiesToSpawnArray[0] += 1;
            enemiesToSpawnArray[1] += 1;
            enemiesToSpawnArray[2] += 1;
	}
	private Vector3 GenerateSpawnPosition()
	{
		float randomPosX = Random.Range(-20f, 20f);
		float randomPosZ = Random.Range(-20f, 20f);

		Vector3 randomPos = new(randomPosX, 1, randomPosZ);

		return randomPos;
	}
	 
	void SpawnEnemyWave()
	{
		for (int i = 0; i < enemiesToSpawnArray[0]; i++)
		{
			InstantiateEnemy(0);
		}
		for(int i = 0; i < enemiesToSpawnArray[1]; i++)
		{
            InstantiateEnemy(1);
        }
        for (int i = 0; i < enemiesToSpawnArray[2]; i++)
        {
            InstantiateEnemy(2);
        }
    }

	void InstantiateEnemy(int type)
	{
		GameObject instantiatedEnemy = Instantiate(enemy[type], GenerateSpawnPosition(), Quaternion.Euler(90, 0, 0));
		instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent
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
