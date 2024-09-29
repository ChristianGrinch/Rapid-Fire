using System.Collections;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    private float powerupLength = 5f;

    public AudioClip powerupCollectSound;
    public AudioClip powerupExpireSound;
    AudioSource audioData;

    private void Start()
    {
        audioData = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            audioData.clip = powerupCollectSound;
            audioData.volume = 0.3f;
            audioData.Play();
            StartCoroutine(ApplySpeedPowerup(other.gameObject));
        }
    }

    private IEnumerator ApplySpeedPowerup(GameObject player)
    {
        // Apply speed boost
        player.GetComponent<PlayerController>().speed = 150;
 
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(powerupLength);

        audioData.clip = powerupExpireSound;
        audioData.volume = 0.3f;
        audioData.Play();

        // Revert the effect after the powerup duration
        player.GetComponent<PlayerController>().speed = 80;


        Destroy(gameObject, powerupExpireSound.length);
    }
}
