using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public int health = 100;
    public int maxHealth = 200;


    public void UpdateHealth(int newHealth)
    {
        if(newHealth <= maxHealth) // Makes sure health never goes above 200
        {
            health = newHealth;
        }
        else if (newHealth > maxHealth && gameObject.name == "Player")
        {
            health = maxHealth;
        }

        if (newHealth <= 0) // Makes sure health never goes below 0
        {
            health = 0;
        }

        if (health <= 0 && gameObject.name != "Player")
        {
            Destroy(gameObject);
        }
        else if(health <= 0 && gameObject.name == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
