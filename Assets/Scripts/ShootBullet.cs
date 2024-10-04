using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    public float speed = 1;
    public int range = 50;
    public int damage = 10;

    private Vector3 spawnPos;
    private Vector3 currentPos;
    private Rigidbody bulletRb;
    public GameObject ammoParent;

    public GameObject ammo;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        bulletRb = GetComponent<Rigidbody>();
        ammoParent = GameObject.Find("Ammo Piles");
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;

        if (Vector3.Distance(currentPos, spawnPos) > range)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(currentPos, spawnPos) <= range)
        {
            bulletRb.AddRelativeForce(speed * Time.deltaTime * Vector3.up, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            if (collision.gameObject.TryGetComponent<HealthSystem>(out var enemyHealth)) // Makes sure that the line above was able to actually get the HealthSystem script
            {
                int modifiedHealth = enemyHealth.health - damage; // Creates a modified health variable, and makes the value equal to the enemy health minus the damage the bullet should do.
                enemyHealth.UpdateHealth(modifiedHealth); // Calls the function inside of HealthSystem inside the enemy to update the health on the enemy using the modifiedHealth.
                if (modifiedHealth <= 0)
                {
                    GameObject instantiatedEnemy = Instantiate(ammo, bulletRb.transform.position, Quaternion.Euler(90, 0, 0)); // Spawns ammo on enemy death
                    instantiatedEnemy.transform.parent = ammoParent.transform; // Sets parent
                    StartCoroutine(EnemySpawnManager.Instance.AssignEnemiesToArray());
                }
            }
            else // If it isn't able to get the HealthSystem script, throw an error.
            {
                Debug.LogError("couldnt locate healthsystem script on enemy");
            }

        }
    }

    public void UpdateStats(int newDamage, int newRange, float newSpeed)
    {
        damage = newDamage;
        range = newRange;
        speed = newSpeed;
    }
}
