using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int damage = 50;

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
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();

            if (playerHealth != null) 
            {
                int modifiedHealth = playerHealth.health - damage; 
                playerHealth.UpdateHealth(modifiedHealth); 
            }
            else 
            {
                Debug.LogError("couldnt locate healthsystem script on player");
            }
        }
    }
}
