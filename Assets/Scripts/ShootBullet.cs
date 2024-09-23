using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    private int speed = 1; // Speed of the bullet --IMPLIMENTED
    private int range = 50; // How far the bullet can travel from its initial location before it despawns --IMPLIMENTED
    private int damage = 10; // damage of bullet obv --IMPLIMENTED

    private Vector3 spawnPos;
    private Vector3 currentPos;
    private Rigidbody bulletRb;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        bulletRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;

        if (Vector3.Distance(currentPos, spawnPos) > range)
        {
            Destroy(gameObject);
        }
        else
        {
            bulletRb.AddRelativeForce(Vector3.up * speed * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Powerup"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            HealthSystem enemyHealth = collision.gameObject.GetComponent<HealthSystem>(); // Gets the HealthSystem script from the enemy.

            if(enemyHealth != null) // Makes sure that the line above was able to actually get the HealthSystem script
            {
                int modifiedHealth = enemyHealth.health - damage; // Creates a modified health variable, and makes the value equal to the enemy health minus the damage the bullet should do.
                enemyHealth.UpdateHealth(modifiedHealth); // Calls the function inside of HealthSystem inside the enemy to update the health on the enemy using the modifiedHealth.
            }
            else // If it isn't able to get the HealthSystem script, throw an error.
            {
                Debug.LogError("couldnt locate healthsystem script on enemy");
            }

        }
    }
}
