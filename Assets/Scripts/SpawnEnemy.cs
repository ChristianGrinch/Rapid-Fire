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

	private int[] enemyTypeArray = new int[] {4, 0, 0};

    // Start is called before the first frame update
    void Start()
	{
		SpawnEnemyWave(currentWave);
		InstantiateEnemy();
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log("Current wave: " + currentWave);
		enemyCountArray = GameObject.FindGameObjectsWithTag("Enemy");
		enemyCount = enemyCountArray.Length;

		if (enemyCount == 0)
		{
			currentWave++;
			SpawnEnemyWave(currentWave);
			InstantiateEnemy();
            //EnemyTypeToSpawn(); ITLL MAYBE BE HERE. FINISH CODE LATER.

        }

	}

	void EnemyTypeToSpawn()
	{
			currentWave++;
			enemyTypeArray[0] += 1;
            enemyTypeArray[1] += 1;
            enemyTypeArray[2] += 1;
	}
	private Vector3 GenerateSpawnPosition()
	{
		float randomPosX = Random.Range(-20f, 20f);
		float randomPosZ = Random.Range(-20f, 20f);

		Vector3 randomPos = new(randomPosX, 1, randomPosZ);

		return randomPos;
	}
	 
	void SpawnEnemyWave(int enemiesToSpawn)
	{
		for (int i = 0; i < enemiesToSpawn; i++)
		{
			InstantiateEnemy();
		}
	}

	void InstantiateEnemy()
	{
		enemyType = enemy.Length;
		randomEnemy = Random.Range(0, enemyType);
		GameObject instantiatedEnemy = Instantiate(enemy[randomEnemy], GenerateSpawnPosition(), Quaternion.Euler(90, 0, 0));
		instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent
	}
}
