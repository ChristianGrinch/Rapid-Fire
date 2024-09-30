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
    private float nextSpawnTime = 0;

    public GameObject powerupParent;
    public GameObject ammoParent;
   
    private int newWave = 1;

    public List<GameObject> ammunition;
    public List<GameObject> heartPowerups;
    public List<GameObject> speedPowerups;

    // Update is called once per frame
    void Update()
    {
        randomSpawnPos = new(randomXPos, 1, randomZPos);
        int currentWave = EnemySpawnManager.Instance.currentWave;
        if (UIManager.Instance.isGameActive && UIManager.Instance.didPlayerLoadPowerupManager)
        {
            SpawnPowerupsOnLoad();
            UIManager.Instance.didPlayerLoadPowerupManager = false;

        } 
        else if (Time.time >= nextSpawnTime || currentWave >= newWave)
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


    public void SpawnPowerupsOnLoad()
    {
        for (int i = 0; i < ammunition.Count; i++)
        {
            GenerateRandomPos();
            InstantiateObject(ammo, randomSpawnPos, ammoParent);
        }
        for (int i = 0; i < heartPowerups.Count; i++)
        {
            GenerateRandomPos();
            InstantiateObject(heartPowerup, randomSpawnPos, powerupParent);
        }
        for (int i = 0; i < speedPowerups.Count; i++)
        {
            GenerateRandomPos();
            InstantiateObject(speedPowerup, randomSpawnPos, powerupParent);
        }
    }

    void AssignPowerupsToList(GameObject instantiatedObject, GameObject objectToSpawn)
    {
        ammunition = new();
        heartPowerups = new();
        speedPowerups = new();

        if (objectToSpawn.ToString() == "ammo")
        {
            ammunition.Add(instantiatedObject);
        }
        else if (objectToSpawn.ToString() == "heartPowerup")
        {
            heartPowerups.Add(instantiatedObject);
        }
        else if (objectToSpawn.ToString() == "speedPowerup")
        {
            speedPowerups.Add(instantiatedObject);
        }
    }

    void InstantiateObject(GameObject objectToSpawn, Vector3 spawnPos, GameObject objectParent)
    {
        GameObject instantiatedObject = Instantiate(objectToSpawn, randomSpawnPos, Quaternion.Euler(90, 0, 0));
        instantiatedObject.transform.parent = objectParent.transform; // Sets parent
        AssignPowerupsToList(instantiatedObject, objectToSpawn);
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
