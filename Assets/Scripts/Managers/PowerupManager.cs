using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public GameObject heartPowerup;
    public GameObject ammo;
    public GameObject speedPowerup;

    private float randomXPos;
    private float randomZPos;
    private Vector3 randomSpawnPos;

    private float spawnInterval = 30;
    private float nextSpawnTime = 0;

    public GameObject powerupParent;
    public GameObject ammoParent;
   
    private int newWave = 1;

    // Update is called once per frame
    void Update()
    {
        randomSpawnPos = new(randomXPos, 1, randomZPos);
        int currentWave = EnemySpawnManager.Instance.currentWave;

        //Debug.Log(currentWave);
        if (Time.time >= nextSpawnTime || currentWave >= newWave)
        {
            GenerateRandomPos();
            InstantiateObject(ammo, randomSpawnPos, ammoParent);

            GenerateRandomPos();
            InstantiateObject(heartPowerup, randomSpawnPos, powerupParent);

            GenerateRandomPos();
            InstantiateObject(speedPowerup, randomSpawnPos, powerupParent);

            nextSpawnTime = Time.time + spawnInterval;
            newWave++;
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
