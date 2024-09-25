using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHeart : MonoBehaviour
{

    private int healthBoost = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthSystem playerHealth = other.gameObject.GetComponent<HealthSystem>();

            if (playerHealth != null)
            {
                int modifiedHealth = playerHealth.health + healthBoost;
                playerHealth.UpdateHealth(modifiedHealth);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("couldnt locate healthsystem script on player");
            }
        }
    }
}
