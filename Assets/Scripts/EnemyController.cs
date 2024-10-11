using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{

	private int damage;
	private float speed;
	public int exp;
	public int health;
	private int difficulty = 1;
	private float damageRate = 0.4f;
	private bool isDamagingPlayer = false;
	private int jumpForce = 10;
	private bool isRunningJump;

	private Rigidbody enemyRb;
	private GameObject player;
	private HealthSystem healthSystem;

    public GameObject ringPrefab;
    private float ringExpandTime = 1f;
    private float ringMaxSize = 45f;
    private float spawnRingCooldown = 10f;
    private float lastSpawnRingTime = 0f;
	private bool isDashing = false;

    private int[] rangeDashSeconds = {10, 20};
	private float dashSpeed;

	private NavMeshAgent agent;

    void Start()
	{
		agent = GetComponent<NavMeshAgent>();

		enemyRb = GetComponent<Rigidbody>();
		player = GameObject.FindWithTag("Player");
		healthSystem = gameObject.GetComponent<HealthSystem>();
		AssignStats();
		healthSystem.health = health;

		switch (UIManager.Instance.difficulty)
		{
			case 1:
				rangeDashSeconds[0] = 10;
				rangeDashSeconds[1] = 20;
                dashSpeed = speed * 100;
                break;
            case 2:
                rangeDashSeconds[0] = 8;
                rangeDashSeconds[1] = 15;
                dashSpeed = speed * 120;
                break;
            case 3:
                rangeDashSeconds[0] = 8;
                rangeDashSeconds[1] = 10;
                dashSpeed = speed * 140;
                break;
        }
    }
	IEnumerator Jump(float jumpHeight)
	{
		isRunningJump = true;
        
        yield return new WaitForSeconds(Random.Range(5, 14)); // Delay between jumps
        agent.enabled = false;
        enemyRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f); // Wait 0.2 seconds so Y is > jumpHeight

        while (transform.position.y > jumpHeight)
        {
            yield return null;
        }

		isRunningJump = false;
		agent.enabled = true;
        StartCoroutine(SpawnRing());
    }
    IEnumerator SpawnRing() // not my code, chatgpt
    {
        lastSpawnRingTime = Time.time;
        GameObject ring = Instantiate(ringPrefab, transform.position, Quaternion.identity);

        ring.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        float currentTime = 0f;
        Vector3 initialScale = ring.transform.localScale;

        while (currentTime < ringExpandTime)
        {
            float progress = currentTime / ringExpandTime;
            float scale = Mathf.Lerp(1f, ringMaxSize, progress);
            ring.transform.localScale = new Vector3(scale, ring.transform.localScale.y, scale);

            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(ring, 0.5f);
    }
    IEnumerator Dash()
    {
		isDashing = true;
        yield return new WaitForSeconds(Random.Range(rangeDashSeconds[0], rangeDashSeconds[1]));

        Vector3 direction = (player.transform.position - transform.position).normalized;
        
        enemyRb.AddForce(Time.deltaTime * dashSpeed * direction, ForceMode.Impulse);
		isDashing = false;
    }
    void Update()
	{
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }

        if (!isRunningJump && gameObject.name == "Boss 1")
		{
			StartCoroutine(Jump(2.1f));
		}

		if(!isDashing && gameObject.name == "Enemy 3")
		{
			StartCoroutine(Dash());
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();

			if (playerHealth != null && !isDamagingPlayer)
			{
				StartCoroutine(DamagePlayer(collision, playerHealth));
			}
			else
			{
				Debug.LogError("Unable to locate HealthSystem on player!");
			}	
		}
    }
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			isDamagingPlayer = false;
			StopAllCoroutines();
		}
	}

	IEnumerator DamagePlayer(Collision collision, HealthSystem playerHealth)
	{
		isDamagingPlayer = true;

		int initialHealth = playerHealth.health - damage;
		playerHealth.UpdateHealth(initialHealth);

		while (playerHealth.health > 0)
		{
			int modifiedHealth = playerHealth.health - damage;
			yield return new WaitForSeconds(damageRate);
			playerHealth.UpdateHealth(modifiedHealth);
			Debug.Log("running DamagePlayer in if");
		}
		isDamagingPlayer = false;
	}

	void AssignStats()
	{
		difficulty = UIManager.Instance.difficulty;

		switch (difficulty)
		{
			case 1:
				SetStats(20, 23, 10, 40, "Enemy 1");
				SetStats(25, 21, 15, 60, "Enemy 2");
				SetStats(50, 21, 20, 100, "Enemy 3");
				SetStats(90, 16, 200, 450, "Boss 1");
				break;
			case 2:
				SetStats(25, 23, 15, 55, "Enemy 1");
				SetStats(30, 21, 20, 75, "Enemy 2");
				SetStats(55, 21, 25, 130, "Enemy 3");
				SetStats(95, 16.5f, 250, 675, "Boss 1");
				break;
			case 3:
				SetStats(35, 25, 18, 68, "Enemy 1");
				SetStats(45, 24, 25, 100, "Enemy 2");
				SetStats(70, 24, 35, 156, "Enemy 3");
				SetStats(130, 17.5f, 325, 800, "Boss 1");
				break;
			default:
				Debug.LogError("Invalid difficulty level.");
				break;
		}
	}
	void SetStats(int enemyDamage, float enemySpeed, int enemyExp, int enemyHealth, string enemyName)
	{
		if(gameObject.name == enemyName)
		{
			damage = enemyDamage;
			speed = enemySpeed;
			exp = enemyExp;
			health = enemyHealth;
		}
	}
}
