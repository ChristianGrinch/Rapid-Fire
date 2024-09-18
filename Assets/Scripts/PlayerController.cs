using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        Vector3 currentRotation = transform.rotation.eulerAngles;

        // From HERE -----

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

        // To HERE is NOT MY CODE!! -----
    }

    // Moves player based on WASD/Arrow keys input
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        playerRb.AddForce(Vector3.forward * speed * verticalInput, ForceMode.Impulse);
        playerRb.AddForce(Vector3.right * speed * horizontalInput, ForceMode.Impulse);
        //Debug.Log(playerRb.velocity);
    }
}
