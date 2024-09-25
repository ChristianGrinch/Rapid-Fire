using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerup : MonoBehaviour
{
    public GameObject powerupHeart;
    public GameObject ammo;

    private float randomXPos;
    private float randomZPos;
    private Vector3 randomSpawnPos;

    private float spawnInterval = 30;
    private float nextSpawnTime = 0;

    public GameObject powerupParent;
    public GameObject ammoParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        randomSpawnPos = new(randomXPos, 1, randomZPos);

        if (Time.time >= nextSpawnTime)
        {
            GenerateRandomPos();
            InstantiateObject(ammo, randomSpawnPos, ammoParent);

            GenerateRandomPos();
            InstantiateObject(powerupHeart, randomSpawnPos, powerupParent);

            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    void InstantiateObject(GameObject objectToSpawn, Vector3 spawnPos, GameObject objectParent)
    {
        GameObject instantiatedObject = Instantiate(objectToSpawn, randomSpawnPos, Quaternion.Euler(90, 0, 0));
        instantiatedObject.transform.parent = objectParent.transform; // Sets parent
    }

    Vector3 GenerateRandomPos()
    {
        randomXPos = Random.Range(-20f, 20f);
        randomZPos = Random.Range(-20f, 20f);
        randomSpawnPos = new(randomXPos, 1, randomZPos);

        return randomSpawnPos;
    }
}
