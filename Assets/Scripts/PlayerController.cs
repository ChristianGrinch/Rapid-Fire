using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody playerRb;
    public float speed = 8;
	public float jumpForce = 20;
	public int exp;
	public int health;
	public int lives;
	public int wave;
	public int[] ammo;

	public GameObject gameManager;
	private HealthSystem healthSystem;
	private EnemySpawnManager enemySpawnManager;
	private GunController gunController;

	public bool isGrounded;

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

        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Vector3 rayDirection = Vector3.down;
        float rayDistance = 0.8f;
        Ray ray = new(rayOrigin, rayDirection);

		isGrounded = Physics.Raycast(ray, out RaycastHit hit, rayDistance) && hit.collider.gameObject.layer != LayerMask.NameToLayer("MoveableObject");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            

            Debug.DrawRay(rayOrigin, rayDirection, Color.blue, 5);

			if (Physics.Raycast(ray, rayDistance))
            {
				if(hit.collider.gameObject.layer == LayerMask.NameToLayer("MoveableObject")){
                    Debug.Log("hit a moveable object. no jump :p");
				}
				else
				{
                    Debug.Log("detected ground");
                    Debug.DrawRay(rayOrigin, rayDirection, Color.red, 5);
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }

			}
        }

    }

	private void FixedUpdate()
	{
		if (UIManager.Instance.isGameUnpaused)
		{
			MovePlayer();
			RotatePlayer();
		}
		if (!isGrounded)
		{
			playerRb.AddForce(Vector3.down * 1.5f, ForceMode.Impulse);
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

    public void UpdateSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
