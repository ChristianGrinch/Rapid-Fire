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
	public GameObject[] enemies; // All enemy prefabs
	public GameObject[] enemyCountArray; // All active enemy game objects
	public int totalEnemyCount;

	// Lists of how many enemies there are for each type ([0] is enemy 1, and the value is how many)
	public List<int> enemyCount = new(5);
	private void Update()
	{
		enemyCountArray = EnemySpawnManager.Instance.enemyCountArray;
		totalEnemyCount = enemyCountArray.Length;
	}
	public IEnumerator AssignEnemiesToLists()
	{
		yield return null;
		enemyCount = new() { 0, 0, 0, 0, 0 };

		foreach (GameObject enemy in enemyCountArray)
		{
			if (enemy.name.Contains("Enemy 1"))
			{
				enemyCount[0]++;
			}
			else if (enemy.name.Contains("Enemy 2"))
			{
				enemyCount[1]++;
			}
			else if (enemy.name.Contains("Enemy 3"))
			{
				enemyCount[2]++;
			}
			else if (enemy.name.Contains("Boss 1"))
			{
				enemyCount[3]++;
			}
			else if (enemy.name.Contains("Ice Zombie"))
			{
				enemyCount[4]++;
			}
		}
	}
}
