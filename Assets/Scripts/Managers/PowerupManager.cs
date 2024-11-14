using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static ItemData;

public class PowerupManager : MonoBehaviour
{
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
	public GameObject ammo;
    public GameObject heartPowerup;
    public GameObject speedPowerup;

    private float randomXPos;
    private float randomZPos;
    private Vector3 randomSpawnPos;

    private float spawnInterval = 30;
    private float nextSpawnTime = 30;

    private GameObject powerupParent;
	private GameObject ammoParent;
   
    private int newWave = 1;

    public int ammunition;
    public int heartPowerups;
    public int speedPowerups;

    public bool isLoading = false;
    public bool didLoad = false;

    private float mapSize = GameManager.mapSize;
	
	// speed powerup stuff
	private PlayerController playerController;
	private GameObject player;
	private float powerupLength = 5f;
	public AudioClip powerupCollectSound;
	public AudioClip powerupExpireSound;
	private AudioSource audioData;
	public bool runningSpeedPowerup;

	public GameObject[] powerups;
	private void Start()
	{
		player = GameManager.Instance.player;
		powerupParent = GameManager.Instance.powerups;
		ammoParent = GameManager.Instance.ammo;

		audioData = GetComponent<AudioSource>();
		playerController = player.GetComponent<PlayerController>();
	}
	void Update()
    {
		powerups = GameObject.FindGameObjectsWithTag("Powerup");
		randomSpawnPos = new(randomXPos, 1, randomZPos);
        int currentWave = EnemySpawnManager.Instance.currentWave;
        if (!GameManager.Instance.isGamePaused)
        {
            if (GameManager.Instance.didLoadPowerupManager)
            {
                SpawnPowerupsOnLoad();
                GameManager.Instance.didLoadPowerupManager = false;
                newWave = currentWave;
            }
            else if(Time.time >= nextSpawnTime || currentWave > newWave)
            {
                randomSpawnPos = GenerateRandomPos();
                InstantiateObject(ammo, randomSpawnPos, ammoParent);

                randomSpawnPos = GenerateRandomPos();
                InstantiateObject(heartPowerup, randomSpawnPos, powerupParent);

                randomSpawnPos = GenerateRandomPos();
                InstantiateObject(speedPowerup, randomSpawnPos, powerupParent);

                nextSpawnTime = Time.time + spawnInterval;
                newWave++;
            }
        }
    }
    Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomPosition = new(
            Random.Range(-mapSize, mapSize),
            Random.Range(0, mapSize),
            Random.Range(-mapSize, mapSize)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, EnemySpawnManager.Instance.spawnBufferDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return randomPosition;
    }
    public void SpawnPowerupsOnLoad()
    {
        didLoad = true;
        isLoading = true;

		List<PowerupType> powerupTypes = GameManager.Instance.savedPowerupTypes;
		List<Vector3> powerupPositions = GameManager.Instance.savedPowerupPositions;

		for(var i = 0; i < powerupTypes.Count; i++)
		{
			switch (powerupTypes[i])
			{
				case PowerupType.Ammo:
					InstantiateObject(ammo, powerupPositions[i], ammoParent);
					break;
				case PowerupType.Health:
					InstantiateObject(heartPowerup, powerupPositions[i], ammoParent);
					break;
				case PowerupType.Speed:
					InstantiateObject(speedPowerup, powerupPositions[i], ammoParent);
					break;
			}
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
        GameObject instantiatedObject = Instantiate(objectToSpawn, spawnPos, Quaternion.Euler(90, 0, 0));
        instantiatedObject.transform.parent = objectParent.transform; // Sets parent
        instantiatedObject.name = objectToSpawn.name; // Removes (Clone) from name
        if (isLoading == false)
        {
            AssignPowerupsToList(objectToSpawn);
        }  
    }
    Vector3 GenerateRandomPos()
    {
        Vector3 randomSpawnPos = GetRandomNavMeshPosition();
        randomSpawnPos = new(randomSpawnPos.x, 0.5f, randomSpawnPos.z);

        return randomSpawnPos;
    }
	public IEnumerator ApplySpeedPowerup()
	{
		runningSpeedPowerup = true;
		playerController.speedPowerupCount--;

		// Apply speed boost
		player.GetComponent<PlayerController>().speed = 150;

		audioData.clip = powerupCollectSound;
		audioData.volume = 0.3f;
		audioData.Play();

		yield return new WaitForSeconds(powerupLength);

		audioData.clip = powerupExpireSound;
		audioData.volume = 0.3f;
		audioData.Play();

		// Revert the effect after the powerup duration
		player.GetComponent<PlayerController>().speed = 80;

		speedPowerups--;
		runningSpeedPowerup = false;
	}
	public PowerupData GetPowerupData()
	{
		List<Vector3> powerupPositions = new();
		List<PowerupType> powerupTypes = new();
		foreach (var powerup in powerups)
		{
			powerupPositions.Add(powerup.transform.position);

			switch (powerup.name)
			{
				case "Ammo":
					powerupTypes.Add(PowerupType.Ammo);
					break;
				case "Powerup Heart":
					powerupTypes.Add(PowerupType.Health);
					break;
				case "SpeedPowerup":
					powerupTypes.Add(PowerupType.Speed);
					break;
				default:
					Debug.LogWarning("Unrecognized powerup name: " + powerup.name);
					break;
			}
		}
		return new PowerupData { Positions = powerupPositions, Types = powerupTypes};
	}
}
public class PowerupData
{
	public List<Vector3> Positions { get; set; }
	public List<PowerupType> Types { get; set; }
}