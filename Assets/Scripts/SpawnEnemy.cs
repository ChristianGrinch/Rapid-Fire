using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemy;

    private float randomXPos;
    private float randomZPos;
    private Vector3 randomVector3;

    private float spawnInterval = 1;
    private float nextSpawnTime = 0;
    private int enemyLength;
    private int randomEnemy;

    public GameObject enemyParent;

    private void Awake()
    {
        randomXPos = Random.Range(-20, 20);
        randomZPos = Random.Range(-20, 20);
        enemyLength = enemy.Length;
        randomEnemy = Random.Range(0, enemyLength);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        randomVector3 = new(randomXPos, 1, randomZPos);
        randomEnemy = Random.Range(0, enemyLength);

        if (Time.time >= nextSpawnTime)
        {
            GameObject instantiatedEnemy = Instantiate(enemy[randomEnemy], randomVector3, Quaternion.Euler(90, 0, 0));
            instantiatedEnemy.transform.parent = enemyParent.transform; // Sets parent

            nextSpawnTime = Time.time + spawnInterval;

            randomXPos = Random.Range(-20f, 20f);
            randomZPos = Random.Range(-20f, 20f);
        }
    }
}
