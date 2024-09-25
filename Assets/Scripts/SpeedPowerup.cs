using System.Collections;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    public float powerupLength = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ApplySpeedPowerup(other.gameObject));
        }
    }

    private IEnumerator ApplySpeedPowerup(GameObject player)
    {
        // Apply speed boost (do stuff)
        // Example: player.GetComponent<PlayerMovement>().speed *= 2;
        player.GetComponent<PlayerController>().speed *= 2;

        yield return new WaitForSeconds(powerupLength);

        // Revert the effect after the powerup duration (do stuff pt2)
        // Example: player.GetComponent<PlayerMovement>().speed /= 2;
        player.GetComponent<PlayerController>().speed /= 2;
    }
}
