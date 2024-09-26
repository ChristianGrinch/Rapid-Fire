using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	private int damage = 50;
	private float speed = 10;
	private float attractRadius = 15;
	private bool isFollowingPlayer = false;
	private bool shouldEnemymove = false; // Exists solely to make sure MoveEnemy is only called in fixed update; Optimization

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
		if (!isFollowingPlayer && Vector3.Distance(player.transform.position, transform.position) < attractRadius)
		{
			shouldEnemymove = true;
		}
	}

	private void FixedUpdate()
	{
		if (shouldEnemymove)
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
				Debug.LogError("couldnt locate healthsystem script on player");
			}
		}

		if (collision.gameObject.CompareTag("Bullet"))
		{
			if (!isFollowingPlayer)
			{
				StartCoroutine(MoveForTime(5));
			}
		}
	}

	IEnumerator MoveForTime(float duration)
	{
		isFollowingPlayer = true;
		float timePassed = 0;

		while (timePassed < duration)
		{
			// Move the enemy once per frame
			MoveEnemy();
			timePassed += Time.deltaTime;
			yield return null;  // Wait for the next frame
		}
		isFollowingPlayer = false;
	}

	void MoveEnemy()
	{
		enemyRb.AddForce(Time.deltaTime * speed * (player.transform.position - transform.position).normalized, ForceMode.Impulse);
		shouldEnemymove = false;
	}

	void AssignDamage()
	{
		switch (gameObject.name)
		{
			case "Enemy 1":
				damage = 20;
				Debug.Log("assign damage enemy 1");
				break;
			case "Enemy 2":
				damage = 25;
				Debug.Log("assign damage enemy 2");
				break;
			case "Enemy 3":
				damage = 50;
				Debug.Log("assign damage enemy 3");
				break;
			case "Boss 1":
				damage = 90;
				Debug.Log("assign damage boss 1");
				break;
			default:
				Debug.LogError("Couldn't locate any enemies by tag and therefor couldn't assign damage.");
				break;
		}
	}
}
