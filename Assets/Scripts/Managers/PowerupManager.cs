using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public GameObject ammo;
    public GameObject heartPowerup;
    public GameObject speedPowerup;

    private float randomXPos;
    private float randomZPos;
    private Vector3 randomSpawnPos;

    private float spawnInterval = 30;
    private float nextSpawnTime = 30;

    public GameObject powerupParent;
    public GameObject ammoParent;
   
    private int newWave = 1;

    public int ammunition;
    public int heartPowerups;
    public int speedPowerups;

    public bool isLoading = false;
    public bool didLoad = false;

    // Update is called once per frame
    void Update()
    {
        randomSpawnPos = new(randomXPos, 1, randomZPos);
        int currentWave = EnemySpawnManager.Instance.currentWave;
        if (UIManager.Instance.isGameActive)
        {
            if (UIManager.Instance.didPlayerLoadPowerupManager)
            {
                SpawnPowerupsOnLoad();
                UIManager.Instance.didPlayerLoadPowerupManager = false;
                newWave = currentWave;
            }
            else if(Time.time >= nextSpawnTime || currentWave > newWave)
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
    }

    public void SpawnPowerupsOnLoad()
    {
        didLoad = true;
        isLoading = true;

        for (int i = 0; i < ammunition; i++)
        {
            GenerateRandomPos();
            InstantiateObject(ammo, randomSpawnPos, ammoParent);
        }
        for (int i = 0; i < heartPowerups; i++)
        {
            GenerateRandomPos();
            InstantiateObject(heartPowerup, randomSpawnPos, powerupParent);
        }
        for (int i = 0; i < speedPowerups; i++)
        {
            GenerateRandomPos();
            InstantiateObject(speedPowerup, randomSpawnPos, powerupParent);
        }

        isLoading = false;

    }

    void AssignPowerupsToList(GameObject objectToSpawn)
    {
        if (objectToSpawn.name == "Ammo")
        {
            ammunition++;
        }
        else if (objectToSpawn.name == "Powerup Heart")
        {
            heartPowerups++;
        }   
        else if (objectToSpawn.name == "SpeedPowerup")
        {
            speedPowerups++;
        }
    }

    void InstantiateObject(GameObject objectToSpawn, Vector3 spawnPos, GameObject objectParent)
    {
        GameObject instantiatedObject = Instantiate(objectToSpawn, randomSpawnPos, Quaternion.Euler(90, 0, 0));
        instantiatedObject.transform.parent = objectParent.transform; // Sets parent
        instantiatedObject.name = objectToSpawn.name; // Removes (Clone) from name
        if (isLoading == false)
        {
            AssignPowerupsToList(objectToSpawn);
        }  
    }

    Vector3 GenerateRandomPos()
    {
        randomXPos = Random.Range(-20f, 20f);
        randomZPos = Random.Range(-20f, 20f);
        randomSpawnPos = new(randomXPos, 1, randomZPos);

        return randomSpawnPos;
    }

    // Singleton code -----
    public static PowerupManager Instance { get; private set; }

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

    // End singleton code -----
}
