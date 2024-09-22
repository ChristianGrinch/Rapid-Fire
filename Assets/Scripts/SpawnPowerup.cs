using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerup : MonoBehaviour
{
    public GameObject powerupHeart;

    private float randomXPos;
    private float randomZPos;
    private Vector3 randomVector3;

    private float spawnInterval = 10;
    private float nextSpawnTime = 0;

    private void Awake()
    {
        randomXPos = Random.Range(-20, 20);
        randomZPos = Random.Range(-20, 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        randomVector3 = new(randomXPos, 1, randomZPos);

        if (Time.time >= nextSpawnTime)
        {
            Instantiate(powerupHeart, randomVector3, Quaternion.Euler(90, 0, 0));

            nextSpawnTime = Time.time + spawnInterval;

            randomXPos = Random.Range(-20f, 20f);
            randomZPos = Random.Range(-20f, 20f);
        }
    }
}
