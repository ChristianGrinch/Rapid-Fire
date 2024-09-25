using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 80;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Sprinting();
        //Debug.Log(speed);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
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
        // Get mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert screen position to world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.y));

        // Calculate the direction from player to mouse cursor
        Vector3 direction = mouseWorldPosition - transform.position;

        // Lock rotation to only affect the Y-axis (so the player stays upright)
        direction.y = 0;

        // Apply rotation to player to face the cursor
        if (direction.magnitude > 0.1f) // Prevent errors when the direction is too small
        {
            playerRb.rotation = Quaternion.LookRotation(direction);
        }
    }

    void Sprinting()
    {
        if (speed <= 100)
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
