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
	// All enemy prefabs
	public GameObject[] enemies;
	// Total enemy count
	public GameObject[] enemyCountArray;

	// Lists of how many enemies there are
	public List<List<GameObject>> enemyCounts = new List<List<GameObject>>();
	public List<GameObject> level1Enemies = new();
	public List<GameObject> level2Enemies = new();
	public List<GameObject> level3Enemies = new();
	public List<GameObject> boss1Enemies = new();
	public List<GameObject> iceZombie = new();
	private void Update()
	{
		enemyCountArray = EnemySpawnManager.Instance.enemyCountArray;
	}
	public IEnumerator AssignEnemiesToLists()
	{
		yield return null;
		level1Enemies = new();
		level2Enemies = new();
		level3Enemies = new();
		boss1Enemies = new();
		iceZombie = new();

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
			else if (enemy.name.Contains("Ice Zombie"))
			{
				iceZombie.Add(enemy);
			}
		}
	}
}
