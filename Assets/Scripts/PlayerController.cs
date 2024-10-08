using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody playerRb;
    public float speed = 8;
	public float jumpForce = 10;
	public int exp;
	public int health;
	public int lives;
	public int wave;
	public int[] ammo;

	public GameObject gameManager;
	private HealthSystem healthSystem;
	private EnemySpawnManager enemySpawnManager;
	private GunController gunController;
	public GameObject jumpTrigger;

	private bool isGrounded;
	private int fallMultiplier = 4;

	// Start is called before the first frame update
	void Start()
	{
		playerRb = GetComponent<Rigidbody>();
		healthSystem = GetComponent<HealthSystem>();
		enemySpawnManager = gameManager.GetComponent<EnemySpawnManager>();
		gunController = GetComponent<GunController>();
    }

	// Update is called once per frame
	void Update()
	{
        Sprinting();
		health = healthSystem.health;
		lives = healthSystem.lives;
		wave = enemySpawnManager.currentWave;
		ammo = gunController.ammo;

		if (Input.GetKeyDown(KeyCode.Space) && JumpCheck.Instance.canJump)
		{
			StartCoroutine(Jump());
		}
	}

	private void FixedUpdate()
	{
		isGrounded = JumpCheck.Instance.canJump;

		if (!isGrounded)
		{
			playerRb.AddForce((fallMultiplier - 1) * playerRb.mass * Physics.gravity);
		}

		if (UIManager.Instance.isGameUnpaused)
		{
            MovePlayer();
            RotatePlayer();
        }
	}
	// Moves player based on WASD/Arrow keys input
	void MovePlayer()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		
		playerRb.AddForce(speed * Time.deltaTime * verticalInput * Vector3.forward, ForceMode.Impulse);
		playerRb.AddForce(horizontalInput * speed * Time.deltaTime * Vector3.right, ForceMode.Impulse);
	}

	void RotatePlayer() // all code in this method was made by chatgpt not me :fade:
	{
		if (UIManager.Instance.isGameUnpaused)
		{
			Vector3 mouseScreenPosition = Input.mousePosition; // Get mouse position in screen space
			Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.y)); // Convert screen position to world space
			Vector3 direction = mouseWorldPosition - transform.position; // Calculate the direction from player to mouse cursor

			direction.y = 0; // Lock rotation to only affect the Y-axis (so the player stays upright)

			// Apply rotation to player to face the cursor
			if (direction.magnitude > 0.1f) // Prevent errors when the direction is too small
			{
				playerRb.rotation = Quaternion.LookRotation(direction);
			}
        }
    }

	void Sprinting()
	{
		if (speed <= 100) // Stops changes in speed if speed powerup is enabled
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				speed = 100;
			}
			else if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				speed = 80;
			}
		}
	}

    IEnumerator Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerRb.useGravity = true;

        yield return new WaitForSeconds(0.2f); // Wait 0.2 seconds so Y is > jumpHeight

        while (transform.position.y > 0.5f)
        {
            yield return null;
        }

        playerRb.useGravity = false;
    }

    public void UpdateSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
