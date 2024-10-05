using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpImpactRing : MonoBehaviour
{
    public int ringDamage = 40;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                int newHealth = healthSystem.health - ringDamage;
                healthSystem.UpdateHealth(newHealth);
            }
            else
            {
                Debug.LogError("Couldn't locate player health system!");
            }
        }
    }
}
