using System.Collections;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    private float powerupLength = 5f;

    public AudioClip powerupCollectSound;
    public AudioClip powerupExpireSound;
    AudioSource audioData;
	private PlayerController playerController;
	private GameObject player;

    private void Start()
    {
		player = GameObject.Find("Player");
        audioData = GetComponent<AudioSource>();
		playerController = player.GetComponent<PlayerController>();
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

        PowerupManager.Instance.speedPowerups--;
        Destroy(gameObject, powerupExpireSound.length);
    }
}
