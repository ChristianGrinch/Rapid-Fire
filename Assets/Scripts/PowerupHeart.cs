using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHeart : MonoBehaviour
{

    private int healthBoost = 30;

    public AudioClip powerupCollectSound;
    AudioSource audioData;

    private void Start()
    {
        audioData = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthSystem playerHealth = other.gameObject.GetComponent<HealthSystem>();

            if (playerHealth != null)
            {
                int modifiedHealth = playerHealth.health + healthBoost;
                playerHealth.UpdateHealth(modifiedHealth);

                audioData.clip = powerupCollectSound;
                audioData.Play();

                PowerupManager.Instance.heartPowerups--;
                Destroy(gameObject, powerupCollectSound.length);
            }
            else
            {
                Debug.LogError("couldnt locate healthsystem script on player");
            }
        }
    }
}
