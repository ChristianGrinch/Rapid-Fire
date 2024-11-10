using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

	public int health = 100;
	public int maxHealth = 200;
	public int lives;
	public Image healthBar;
	public GameObject healthBarPrefab;
	public GameObject healthBarGameObject;
	public Canvas worldSpaceCanvas;

	private bool didAssignLives = false;
	private void Start()
	{
		healthBarPrefab = HealthBarManager.Instance.GetHealthBarPrefab();
		Debug.Log(healthBarPrefab);
		healthBarGameObject = Instantiate(healthBarPrefab, worldSpaceCanvas.transform);
		healthBarGameObject.transform.localPosition = new Vector3(0, 2, 0);

		healthBar = healthBarGameObject.transform.Find("Health").GetComponent<Image>();
	}
	private void Update()
	{
		Vector3 targetPos = gameObject.transform.position + new Vector3(0, 2, 0);
		healthBarGameObject.transform.position = targetPos;
	}
	public void UpdateHealth(int newHealth)
	{
		healthBar.fillAmount = health / 100f;
		if (newHealth <= maxHealth || gameObject.CompareTag("Enemy")) // Makes sure health never goes above 200
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
            if (lives - 1 > -1)
            {
                lives -= 1;
            }

        }

		if (health <= 0 && gameObject.name != "Player")
		{
			Destroy(gameObject);
		}

		if(lives <= 0 && gameObject.name == "Player")
		{
			gameObject.SetActive(false);
			GameManager.Instance.GameOver();
			Time.timeScale = 0;
		}

		if(health <= 0 && gameObject.name == "Player" & lives > 0)
		{
			health = 100;
		}
		if(newHealth > 200 && gameObject.name == "Player")
		{
			lives++;
			health = newHealth - 200;
		}
	}

	public void UpdateLives(int newLives)
	{
		lives = newLives;
	}
    public void AssignLives()
	{

		if (!GameManager.Instance.isGamePaused && !didAssignLives)
		{
			switch (GameManager.Instance.difficulty)
			{
				case 1:
					lives = 3;
                    break;
				case 2:
					lives = 2;
                    break;
				case 3:
					lives = 1;
                    break;
				default:
					lives = 3;
					Debug.Log("No difficulty selected.");
					break;
			}
		}
		didAssignLives = true;
	}
}
