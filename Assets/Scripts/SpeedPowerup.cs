using System.Collections;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    public AudioClip powerupCollectSound;
    AudioSource audioData;
	private PlayerController playerController;
	private GameObject player;

    private void Start()
    {
		player = GameManager.Instance.player;
        audioData = GetComponent<AudioSource>();
		playerController = player.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
			StartCoroutine(PlayerCollide());
		}
    }
	private IEnumerator PlayerCollide()
	{
		playerController.speedPowerupCount++;

		audioData.clip = powerupCollectSound;
		audioData.volume = 0.3f;
		audioData.Play();

		gameObject.GetComponent<Renderer>().enabled = false;
		gameObject.GetComponent<Collider>().enabled = false;

		yield return new WaitForSeconds(powerupCollectSound.length);
		Destroy(gameObject);
	}
}
