using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHeart : MonoBehaviour
{

    private int healthBoost = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("heart got touched by player");
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();

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
