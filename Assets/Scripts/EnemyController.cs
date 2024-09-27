using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	private int damage;
	private float speed;
	private float detectionRadius = 15;
	private bool followingPlayer = false;

	private Rigidbody enemyRb;
	public GameObject player;

	// Start is called before the first frame update
	void Start()
	{
		enemyRb = GetComponent<Rigidbody>();
		player = GameObject.Find("Player");
		AssignDamage();
	}

	// Update is called once per frame
	void Update()
	{
		if (!followingPlayer && Vector3.Distance(player.transform.position, transform.position) < detectionRadius)
		{
            MoveEnemy();
        }
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();

			if (playerHealth != null) 
			{
				int modifiedHealth = playerHealth.health - damage; 
				playerHealth.UpdateHealth(modifiedHealth); 
			}
			else 
			{
				Debug.LogError("Unable to locate HealthSystem on player!");
			}
		}

		if (collision.gameObject.CompareTag("Bullet"))
		{
			if (!followingPlayer)
			{
				StartCoroutine(MoveForTime(5));
			}
		}
	}

	IEnumerator MoveForTime(float duration)
	{
		followingPlayer = true;
		float timePassed = 0;

		while (timePassed < duration)
		{
			// Move the enemy once per frame
			MoveEnemy();
			timePassed += Time.deltaTime;
			yield return null;  // Wait for the next frame
		}
		followingPlayer = false;
	}

	void MoveEnemy()
	{
		enemyRb.AddForce(Time.deltaTime * speed * (player.transform.position - transform.position).normalized, ForceMode.Impulse);
	}

	void AssignDamage()
	{
		switch (gameObject.name)
		{
			case "Enemy 1":
				damage = 20;
				speed = 12;
				Debug.Log("assign damage enemy 1");
				break;
			case "Enemy 2":
				damage = 25;
                speed = 10;
                Debug.Log("assign damage enemy 2");
				break;
			case "Enemy 3":
				damage = 50;
                speed = 10;
                Debug.Log("assign damage enemy 3");
				break;
			case "Boss 1":
				damage = 90;
                speed = 5;
                Debug.Log("assign damage boss 1");
				break;
			default:
				Debug.LogError("Couldn't locate any enemies by tag.");
				break;
		}
	}
}
