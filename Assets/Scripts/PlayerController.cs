using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody playerRb;
	public float speed = 80;
	public int exp;
	public int health;
	public int lives;

	private HealthSystem healthSystem;

	// Start is called before the first frame update
	void Start()
	{
		playerRb = GetComponent<Rigidbody>();
		healthSystem = GetComponent<HealthSystem>();
    }

	// Update is called once per frame
	void Update()
	{
		Sprinting();
		health = healthSystem.health;
		lives = healthSystem.lives;
	}

	private void FixedUpdate()
	{
		if (UIManager.Instance.isGameActive)
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
		if (UIManager.Instance.isGameActive)
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

	public void UpdateSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
